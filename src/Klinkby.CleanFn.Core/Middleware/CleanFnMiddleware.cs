﻿using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Options;

namespace Klinkby.CleanFn.Core.Middleware;

/// <summary>
///     Function worker middleware that basically just send request/response data to interceptors.
/// </summary>
/// <seealso href="https://microsoft.github.io/code-with-engineering-playbook/observability/correlation-id/" />
// ReSharper disable once ClassNeverInstantiated.Global
internal class CleanFnMiddleware(
    SuccessCounter successCounter,
    ILogger<CleanFnMiddleware> logger,
    IOptions<CleanFnOptions> options,
    IServiceProvider services)
    : IFunctionsWorkerMiddleware
{
    private readonly IFunctionsWorkerInterceptor[] _interceptors =
    [
        new CorrelationInterceptor(logger, services),
        new SecurityHeadersInterceptor(),
        new ExceptionHandlerInterceptor(successCounter, logger, options)
    ];

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var cancellationToken = context.CancellationToken;
        var request = await context.GetHttpRequestDataAsync();
        Exception? pipelineException = null;

        var continuePipeline = await BeforeInvocation(context, request, cancellationToken);
        if (continuePipeline)
        {
            pipelineException = await SafeInvokeNext(context, next);
        }

        await AfterInvocation(context, request, pipelineException, cancellationToken);
    }

    private static async Task<Exception?> SafeInvokeNext(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            try
            {
                await next(context);
            }
            catch (AggregateException ex) when (ex.InnerException != null)
            {
                return ex.InnerException;
            }
        }
        catch (Exception ex)
        {
            return ex;
        }

        return null;
    }

    private async Task<bool> BeforeInvocation(FunctionContext context, HttpRequestData? request,
        CancellationToken cancellationToken)
    {
        if (null == request)
        {
            return true;
        }

        var continuePipeline = true;
        foreach (var interceptor in _interceptors)
        {
            continuePipeline &= await interceptor.OnExecutingAsync(context, request, cancellationToken);
        }

        return continuePipeline;
    }

    private async Task AfterInvocation(FunctionContext context, HttpRequestData? request,
        Exception? pipelineException, CancellationToken cancellationToken)
    {
        var result = context.GetInvocationResult();
        if (null == request)
        {
            return;
        }

        if (result.Value is not HttpResponseData response)
        {
            result.Value = response = await WriteResponse(request, result, cancellationToken);
        }

        foreach (var interceptor in _interceptors)
        {
            await interceptor.OnExecutedAsync(context, request, response, pipelineException, cancellationToken);
        }
    }

    private static async ValueTask<HttpResponseData> WriteResponse(HttpRequestData request, InvocationResult result,
        CancellationToken cancellationToken)
    {
        var res = request.CreateResponse();
        if (null == result.Value)
        {
            res.StatusCode = HttpStatusCode.NoContent;
        }
        else
        {
            await res.WriteAsJsonAsync(result.Value, cancellationToken);
        }

        return res;
    }
}

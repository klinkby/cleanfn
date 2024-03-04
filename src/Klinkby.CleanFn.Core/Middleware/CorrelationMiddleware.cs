using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace Klinkby.CleanFn.Core.Middleware;

/// <summary>
///     Middleware for fetching or generating, logging and responding correlation id.
/// </summary>
/// <remarks>Attach the correlationId to any outgoing requests.</remarks>
/// <seealso href="https://microsoft.github.io/code-with-engineering-playbook/observability/correlation-id/" />
// ReSharper disable once ClassNeverInstantiated.Global
internal partial class CorrelationMiddleware(ILogger<CorrelationMiddleware> logger) : IFunctionsWorkerMiddleware
{
    internal const string CorrelationIdHeader = "X-Correlation-Id";

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var cancellationToken = context.CancellationToken;
        var request = await context.GetHttpRequestDataAsync();
        var correlationId = request!.Headers.TryGetValues(CorrelationIdHeader, out var requestCorrelationId)
            ? requestCorrelationId.First()
            : context.InvocationId;
        context.Items.Add(CorrelationIdHeader, correlationId);
        LogCorrelationId(logger, correlationId);
        try
        {
            await next(context);
        }
        finally
        {
            var result = context.GetInvocationResult();
            if (result.Value is not HttpResponseData res)
            {
                result.Value = res = await WriteResponse(request, result, cancellationToken);
            }

            res.Headers.Add(CorrelationIdHeader, correlationId);
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

    [LoggerMessage(LogLevel.Information, CorrelationIdHeader + " {CorrelationId}")]
    static partial void LogCorrelationId(ILogger logger, string correlationId);
}

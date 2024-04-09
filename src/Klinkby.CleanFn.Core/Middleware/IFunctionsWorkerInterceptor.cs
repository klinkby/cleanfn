namespace Klinkby.CleanFn.Core.Middleware;

/// <summary>
///     Used by <see cref="CleanFnMiddleware" /> to invoke interceptors before and after function execution.
/// </summary>
/// <seealso cref="CorrelationInterceptor" />
/// <seealso cref="SecurityHeadersInterceptor" />
/// <seealso cref="ExceptionHandlerInterceptor" />
internal interface IFunctionsWorkerInterceptor
{
    ValueTask<bool> OnExecutingAsync(FunctionContext context, HttpRequestData request,
        CancellationToken cancellationToken);

    ValueTask OnExecutedAsync(FunctionContext context, HttpRequestData request, HttpResponseData response,
        Exception? pipelineException, CancellationToken cancellationToken);
}

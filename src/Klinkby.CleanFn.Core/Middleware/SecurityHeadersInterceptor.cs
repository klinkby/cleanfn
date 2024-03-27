using Microsoft.Azure.Functions.Worker.Http;

namespace Klinkby.CleanFn.Core.Middleware;

/// <summary>
///     Interceptor for securing responses.
/// </summary>
/// <remarks>Follows recommendation from <see href="https://observatory.mozilla.org/faq/" />.</remarks>
internal class SecurityHeadersInterceptor : IFunctionsWorkerInterceptor
{
    public ValueTask<bool> OnExecutingAsync(FunctionContext context, HttpRequestData request,
        CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(true);
    }

    public ValueTask OnExecutedAsync(FunctionContext context, HttpRequestData request, HttpResponseData response,
        Exception? pipelineException, CancellationToken cancellationToken)
    {
        var headers = response.Headers;
        headers.TryAddWithoutValidation(KnownHeader.ContentSecurityPolicy,
            "default-src 'none'; frame-ancestors 'none'");
        headers.TryAddWithoutValidation(KnownHeader.XContentTypeOptions, "nosniff");
        headers.TryAddWithoutValidation(KnownHeader.StrictTransportSecurity, "max-age=63072000");
        return ValueTask.CompletedTask;
    }
}

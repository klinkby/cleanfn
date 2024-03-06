using Klinkby.CleanFn.Core.Models;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Klinkby.CleanFn.Core.Middleware;

/// <summary>
///     Interceptor for securing responses.
/// </summary>
/// <remarks>Follows recommendation from <see href="https://observatory.mozilla.org/faq/" />.</remarks>
internal partial class SecurityHeadersInterceptor(ILogger logger, IOptions<CleanFnOptions> options)
    : IFunctionsWorkerInterceptor
{
    public ValueTask<bool> OnExecutingAsync(FunctionContext context, HttpRequestData request,
        CancellationToken cancellationToken)

    {
        var continuePipeline = IsSecure(request) || !options.Value.UpgradeInsecureRequestsToPort.HasValue;
        if (!continuePipeline)
        {
            LogUpgrade(logger);
        }

        return ValueTask.FromResult(continuePipeline);
    }

    public ValueTask OnExecutedAsync(FunctionContext context, HttpRequestData request, HttpResponseData response,
        Exception? pipelineException, CancellationToken cancellationToken)
    {
        var headers = response.Headers;
        headers.TryAddWithoutValidation("Content-Security-Policy", "default-src 'none'; frame-ancestors 'none'");
        headers.TryAddWithoutValidation("X-Content-Type-Options", "nosniff");

        if (IsSecure(request))
        {
            headers.TryAddWithoutValidation("Strict-Transport-Security", "max-age=63072000");
        }
        else if (options.Value.UpgradeInsecureRequestsToPort is { } port)
        {
            response.StatusCode = HttpStatusCode.TemporaryRedirect;
            headers.TryAddWithoutValidation("Vary", "Upgrade-Insecure-Requests");
            headers.TryAddWithoutValidation("Location",
                new UriBuilder(request.Url) { Scheme = Uri.UriSchemeHttps, Port = port }.Uri.ToString());
        }

        return ValueTask.CompletedTask;
    }

    private static bool IsSecure(HttpRequestData request)
    {
        return Uri.UriSchemeHttps == request.Url.Scheme;
    }

    [LoggerMessage(LogLevel.Information, "Skip invocation to upgrade insecure request")]
    static partial void LogUpgrade(ILogger logger);
}

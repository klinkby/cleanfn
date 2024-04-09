using System.Globalization;
using System.Net.Http.Headers;

namespace Klinkby.CleanFn.Core.Extensions;

/// <summary>
///     Static methods for writing to <see cref="HttpResponseData" />.
/// </summary>
internal static class HttpResponseDataExtensions
{
    public static async ValueTask<HttpResponseData> WriteProblemDetailsResponse(
        this HttpResponseData res,
        ProblemDetails problemDetails,
        CancellationToken cancellationToken)
    {
        res.Headers.TryAddWithoutValidation(KnownHeader.ContentLanguage, CultureInfo.CurrentUICulture.Name);

        await res.WriteAsJsonAsync(
            problemDetails,
            KnownContentType.ApplicationProblemJson,
            (HttpStatusCode)problemDetails.Status,
            cancellationToken);
        return res;
    }

    public static async ValueTask<HttpResponseData> WriteHealthResponse(
        this HttpResponseData res,
        HealthResponse health,
        int retryAfter,
        CancellationToken cancellationToken)
    {
        var headers = res.Headers;
        headers.TryAddWithoutValidation(KnownHeader.RetryAfter, $"{retryAfter}");
        SetCacheableAndClose(headers, retryAfter);
        await res.WriteAsJsonAsync(
            health,
            KnownContentType.ApplicationHealthJson,
            health.Status == HealthCheckStatus.Fail ? HttpStatusCode.ServiceUnavailable : HttpStatusCode.OK,
            cancellationToken);
        return res;
    }

    public static async Task<HttpResponseData> WriteSwaggerResponse(this HttpResponseData res, Stream swagger,
        string contentType, int maxAge, CancellationToken cancellationToken)
    {
        res.StatusCode = HttpStatusCode.OK;
        var headers = res.Headers;
        headers.TryAddWithoutValidation(KnownHeader.ContentType, contentType);
        SetCacheableAndClose(headers, maxAge);

        await swagger.CopyToAsync(res.Body, cancellationToken);
        return res;
    }

    private static void SetCacheableAndClose(HttpHeaders headers, int maxAge)
    {
        headers.TryAddWithoutValidation(KnownHeader.CacheControl, $"max-age={maxAge}");
        headers.TryAddWithoutValidation(KnownHeader.Connection, "close");
    }
}

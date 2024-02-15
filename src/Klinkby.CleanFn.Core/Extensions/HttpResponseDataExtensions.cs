using System.Globalization;
using System.Net;
using Klinkby.CleanFn.Core.Models;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Klinkby.CleanFn.Core.Extensions;

/// <summary>
///     Static methods for writing to <see cref="HttpResponseData" />.
/// </summary>
public static class HttpResponseDataExtensions
{
    /// <summary>
    /// Write a <see href="https://inadarei.github.io/rfc-healthcheck/" /> to response
    /// </summary>
    /// <param name="health">AspNet health report</param>
    /// <param name="res">Target response</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation.</param>
    /// <returns></returns>
    public static ValueTask WriteTo(
        this HealthReport health,
        HttpResponseData res,
        CancellationToken cancellationToken)
    {
        var headers = res.Headers;
        headers.Add("Cache-Control", "max-age=3600");
        headers.Add("Connection", "close");
        return res.WriteAsJsonAsync(
            Health.FromHealthReport(health),
            Health.ContentType,
            HttpStatusCode.OK,
            cancellationToken);
    }

    internal static ValueTask WriteTo(
        this ProblemDetails problemDetails,
        HttpResponseData res,
        CancellationToken cancellationToken)
    {
        res.Headers.Add("Content-Language", CultureInfo.CurrentUICulture.Name);
        return res.WriteAsJsonAsync(
            problemDetails,
            ProblemDetails.ContentType,
            (HttpStatusCode)problemDetails.Status,
            cancellationToken);
    }
}
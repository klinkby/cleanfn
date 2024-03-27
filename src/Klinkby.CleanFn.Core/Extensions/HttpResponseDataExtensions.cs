using System.Globalization;
using Klinkby.CleanFn.Core.Models;
using Microsoft.Azure.Functions.Worker.Http;

namespace Klinkby.CleanFn.Core.Extensions;

/// <summary>
///     Static methods for writing to <see cref="HttpResponseData" />.
/// </summary>
public static class HttpResponseDataExtensions
{
    internal static ValueTask WriteTo(
        this ProblemDetails problemDetails,
        HttpResponseData res,
        CancellationToken cancellationToken)
    {
        res.Headers.TryAddWithoutValidation(KnownHeader.ContentLanguage, CultureInfo.CurrentUICulture.Name);
        return res.WriteAsJsonAsync(
            problemDetails,
            KnownContentType.ApplicationProblemJson,
            (HttpStatusCode)problemDetails.Status,
            cancellationToken);
    }
}

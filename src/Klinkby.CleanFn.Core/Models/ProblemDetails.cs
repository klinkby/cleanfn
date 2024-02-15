using System.Text.RegularExpressions;
using Klinkby.CleanFn.Core.Middleware;

namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     Data contract for responding problems RFC7807-compliant.
/// </summary>
/// <seealso cref="ExceptionHandlerMiddleware" />
/// <seealso href="https://www.rfc-editor.org/rfc/rfc7807" />
internal partial record ProblemDetails(string Title, string Detail, int Status, string Type)
{
    public const string ContentType = "application/problem+json";

    public static ProblemDetails FromException(Exception ex, int statusCode)
    {
        var matches = PascalCaseWordsRegex().Matches(ex.GetType().Name);
        var title = matches
            .SkipLast(1)
            .Aggregate("", (acc, match) => $"{acc} {match.Value}", acc => acc.TrimStart());
        var problemDetails = new ProblemDetails(
            title,
            ex.Message,
            statusCode,
            $"https://httpstatuses.io/{statusCode}"
        );
        return problemDetails;
    }

    [GeneratedRegex("([A-Z][^A-Z]+)",
        RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant |
        RegexOptions.NonBacktracking)]
    private static partial Regex PascalCaseWordsRegex();
}
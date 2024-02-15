using System.Net;
using Klinkby.CleanFn.Core.Middleware;
using Microsoft.Extensions.Logging;
using ProblemDetails = Klinkby.CleanFn.Core.Models.ProblemDetails;

namespace Klinkby.CleanFn.Core.Extensions;

/// <summary>
///     Static methods relating to <see cref="FunctionContext" />.
/// </summary>
internal static partial class FunctionContextExtensions
{
    public static async Task WriteProblemDetails(
        this FunctionContext context,
        HttpStatusCode httpStatusCode,
        Exception ex)
    {
        var statusCode = (int)httpStatusCode;
        if (statusCode < 400) return;
        var logger = context.GetLogger<ExceptionHandlerMiddleware>();
        if (statusCode >= 500)
            LogResponseError(logger, statusCode, ex.Message);
        else
            LogResponseWarning(logger, statusCode, ex.Message);

        var cancellationToken = context.CancellationToken;
        var req = await context.GetHttpRequestDataAsync();
        var res = req!.CreateResponse();
        var problemDetails = ProblemDetails.FromException(ex, statusCode);
        await problemDetails.WriteTo(res, cancellationToken);
        context.GetInvocationResult().Value = res;
    }

    [LoggerMessage(LogLevel.Warning, "Response {StatusCode} {Message}")]
    private static partial void LogResponseWarning(ILogger logger, int statusCode, string message);

    [LoggerMessage(LogLevel.Error, "Response {StatusCode} {Message}")]
    private static partial void LogResponseError(ILogger logger, int statusCode, string message);
}
using Klinkby.CleanFn.Core.Extensions;
using Klinkby.CleanFn.Core.Models;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Klinkby.CleanFn.Core.Middleware;

/// <summary>
///     Middleware for handling exceptions by responding RFC7807-compliant <see cref="Core.Models.ProblemDetails" />.
/// </summary>
/// <seealso href="https://www.rfc-editor.org/rfc/rfc7807" />
internal partial class ExceptionHandlerInterceptor(ILogger logger, IOptions<CleanFnOptions> options)
    : IFunctionsWorkerInterceptor
{
    public ValueTask<bool> OnExecutingAsync(FunctionContext context, HttpRequestData request,
        CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(true);
    }

    public async ValueTask OnExecutedAsync(FunctionContext context, HttpRequestData request, HttpResponseData response,
        Exception? pipelineException, CancellationToken cancellationToken)
    {
        if (null == pipelineException)
        {
            return;
        }

        var statusCode = options.Value
            .ExceptionMap
            .Where(map => map.ExceptionType.IsInstanceOfType(pipelineException))
            .Select(map => map.StatusCode)
            .DefaultIfEmpty(HttpStatusCode.InternalServerError)
            .First();

        await WriteProblemDetails(pipelineException, statusCode, response, cancellationToken);
    }

    private async ValueTask WriteProblemDetails(
        Exception ex,
        HttpStatusCode httpStatusCode,
        HttpResponseData response,
        CancellationToken cancellationToken)
    {
        var statusCode = (int)httpStatusCode;
        switch (statusCode)
        {
            case < 400:
                return;
            case >= 500:
                LogResponseError(logger, statusCode, ex.Message);
                break;
            default:
                LogResponseWarning(logger, statusCode, ex.Message);
                break;
        }

        var problemDetails = ProblemDetails.FromException(ex, statusCode);
        await problemDetails.WriteTo(response, cancellationToken);
    }

    [LoggerMessage(LogLevel.Warning, "Response {StatusCode} {Message}")]
    private static partial void LogResponseWarning(ILogger logger, int statusCode, string message);

    [LoggerMessage(LogLevel.Error, "Response {StatusCode} {Message}")]
    private static partial void LogResponseError(ILogger logger, int statusCode, string message);
}

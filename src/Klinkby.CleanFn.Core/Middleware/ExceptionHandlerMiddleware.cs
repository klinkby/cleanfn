using Klinkby.CleanFn.Core.Extensions;
using Klinkby.CleanFn.Core.Models;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Options;

namespace Klinkby.CleanFn.Core.Middleware;

/// <summary>
///     Middleware for handling exceptions by responding RFC7807-compliant <see cref="Core.Models.ProblemDetails" />.
/// </summary>
/// <seealso href="https://www.rfc-editor.org/rfc/rfc7807" />
// ReSharper disable once ClassNeverInstantiated.Global
internal class ExceptionHandlerMiddleware : IFunctionsWorkerMiddleware
{
    private readonly IOptions<ExceptionHandlerOptions> _options;

    public ExceptionHandlerMiddleware(IOptions<ExceptionHandlerOptions> options)
    {
        _options = options;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            try
            {
                await next(context);
            }
            catch (AggregateException ex) when (ex.InnerException != null)
            {
                throw ex.InnerException;
            }
        }
        catch (Exception ex)
        {
            var statusCode = _options.Value
                .Map
                .Where(map => map.ExceptionType.IsInstanceOfType(ex))
                .Select(map => map.StatusCode)
                .DefaultIfEmpty(HttpStatusCode.InternalServerError)
                .First();
            await context.WriteProblemDetails(statusCode, ex);
        }
    }
}

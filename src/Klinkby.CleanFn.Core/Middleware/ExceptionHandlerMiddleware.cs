using System.ComponentModel.DataAnnotations;
using System.Net;
using Klinkby.CleanFn.Core.Extensions;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace Klinkby.CleanFn.Core.Middleware;

/// <summary>
///     Middleware for handling exceptions by responding RFC7807-compliant <see cref="Core.Models.ProblemDetails" />.
/// </summary>
/// <seealso href="https://www.rfc-editor.org/rfc/rfc7807" />
// ReSharper disable once ClassNeverInstantiated.Global
internal class ExceptionHandlerMiddleware : IFunctionsWorkerMiddleware
{
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
        catch (ValidationException ex) // TODO Get from Options
        {
            await context.WriteProblemDetails(HttpStatusCode.BadRequest, ex); // 400
        }
        catch (ArgumentException ex)
        {
            await context.WriteProblemDetails(HttpStatusCode.BadRequest, ex); // 400
        }
        catch (UnauthorizedAccessException ex)
        {
            await context.WriteProblemDetails(HttpStatusCode.Unauthorized, ex); // 401
        }
        catch (InvalidOperationException ex)
        {
            await context.WriteProblemDetails(HttpStatusCode.Forbidden, ex); // 403
        }
        catch (HttpRequestException ex)
        {
            await context.WriteProblemDetails(HttpStatusCode.ServiceUnavailable, ex); // 503
        }
        catch (TimeoutException ex)
        {
            await context.WriteProblemDetails(HttpStatusCode.GatewayTimeout, ex); // 504
        }
        catch (Exception ex) // unhandled exception
        {
            await context.WriteProblemDetails(HttpStatusCode.InternalServerError, ex); // 500
        }
    }
}
using Klinkby.CleanFn.Core.Extensions;

namespace Klinkby.CleanFn.Core.Functions;

/// <summary>
///     If <see cref="HealthCheckService" /> is registered, this function will return the health status of the application.
/// </summary>
/// <param name="logger"></param>
/// <param name="healthCheck"></param>
/// <example>
///     <c>
///         services.AddHealthChecks();
///     </c>
/// </example>
public partial class HealthFunction(ILogger<HealthFunction> logger, HealthCheckService? healthCheck = default)
{
    /// <summary>
    /// </summary>
    /// <param name="req"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Function(nameof(HealthGet))]
    public async Task<HttpResponseData> HealthGet(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "health")]
        HttpRequestData req,
        CancellationToken cancellationToken)
    {
        if (default == healthCheck)
        {
            LogServiceMissing(logger);
            return req.Create404Response();
        }

        var health = await healthCheck.CheckHealthAsync(cancellationToken);
        return await req.CreateResponse()
            .WriteHealthResponse(
                HealthResponse.FromHealthReport(health),
                30,
                cancellationToken);
    }

    [LoggerMessage(LogLevel.Warning,
        "Health check request failed because .AddHealthChecks() was not called on startup")]
    private static partial void LogServiceMissing(ILogger logger);
}

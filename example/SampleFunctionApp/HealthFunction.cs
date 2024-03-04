using Klinkby.CleanFn.Core.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SampleFunctionApp;

/// <summary>
///     The Azure Function for observing service health.
/// </summary>
public class HealthFunction(HealthCheckService healthCheck)
{
    private const string Route = "health";

    [Function(nameof(HealthGet))]
    public async Task<HttpResponseData> HealthGet(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = Route)]
        HttpRequestData req,
        CancellationToken cancellationToken)
    {
        var health = await healthCheck.CheckHealthAsync(cancellationToken);
        var res = req.CreateResponse();
        await health.WriteTo(res, cancellationToken);
        return res;
    }
}

using Klinkby.CleanFn.Core.Extensions;
using Klinkby.CleanFn.Core.HealthChecks;
using Klinkby.CleanFn.Core.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SampleFunctionApp;

/// <summary>
///     The Azure Function for observing service health.
/// </summary>
public class HealthFunction(IMediator mediator)
{
    private const string Route = "health";

    [Function(nameof(HealthGet))]
    public async Task<HealthResponse> HealthGet(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = Route)]
        HttpRequestData req,
        CancellationToken cancellationToken)
    {
        var query = new HealthRequest();
        return await mediator.Send<HealthRequest, HealthResponse>(query, cancellationToken);
    }
}

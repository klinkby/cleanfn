using Klinkby.CleanFn.Abstractions;
using Klinkby.CleanFn.Core.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Klinkby.CleanFn.Core.Handlers;

internal class HealthRequestHandler(HealthCheckService healthCheck) : IRequestHandler<HealthRequest, HealthResponse>
{
    public async Task<HealthResponse> Handle(HealthRequest request, CancellationToken cancellationToken)
    {
        var health = await healthCheck.CheckHealthAsync(cancellationToken);
        return HealthResponse.FromHealthReport(health);
    }
}

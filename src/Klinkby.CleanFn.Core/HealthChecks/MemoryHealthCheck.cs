using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Klinkby.CleanFn.Core.HealthChecks;

/// <summary>
///     A health check that reports the current memory usage.
/// </summary>
public class MemoryHealthCheck : IHealthCheck
{
    private const int MegaBitShift = 20; // >>20 == /1,048,576
    private const float Percent100 = 100.0f;
    private const float Percent90 = 90.0f;
    private const float Percent80 = 80.0f;

    /// <summary>
    ///     Default name of the health check.
    /// </summary>
    public const string Name = "Memory";

    /// <summary>
    ///     Report the service process memory consumption.
    /// </summary>
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var allocatedMb = Process.GetCurrentProcess().PrivateMemorySize64 >> MegaBitShift;
        var totalMb = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes >> MegaBitShift;
        if (0 == totalMb)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("Total memory is zero"));
        }

        var allocatedPercent = allocatedMb * Percent100 / totalMb;
        var status = allocatedPercent switch
        {
            >= Percent90 => HealthStatus.Unhealthy,
            >= Percent80 => HealthStatus.Degraded,
            _ => HealthStatus.Healthy
        };
        var result = new HealthCheckResult(status);
        return Task.FromResult(result);
    }
}

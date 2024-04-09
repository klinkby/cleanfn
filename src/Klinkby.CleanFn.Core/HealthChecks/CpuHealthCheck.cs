using System.Diagnostics;

namespace Klinkby.CleanFn.Core.HealthChecks;

/// <summary>
///     A health check that reports the overall cpu usage rate since process start.
///     Sustained CPU pressure over 80% is considered degraded, over 90% unhealthy.
/// </summary>
public class CpuHealthCheck : IHealthCheck
{
    private const float Percent90 = 0.9f;
    private const float Percent80 = 0.8f;

    /// <summary>
    ///     Default name of the health check.
    /// </summary>
    public const string Name = "Cpu";

    /// <summary>
    ///     Report the process cpu usage.
    /// </summary>
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var process = Process.GetCurrentProcess();
        var runtime = DateTime.Now - process.StartTime;
        var cpuUsageRate = process.TotalProcessorTime / runtime / Environment.ProcessorCount;
        var status = cpuUsageRate switch
        {
            >= Percent90 => HealthStatus.Unhealthy,
            >= Percent80 => HealthStatus.Degraded,
            _ => HealthStatus.Healthy
        };
        return Task.FromResult(new HealthCheckResult(status));
    }
}

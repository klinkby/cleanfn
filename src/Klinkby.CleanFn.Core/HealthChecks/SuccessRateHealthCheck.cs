namespace Klinkby.CleanFn.Core.HealthChecks;

/// <summary>
///     A health check that reports the success rate.
///     Success rate over under 50% is considered unhealthy, under 75% degraded.
/// </summary>
public class SuccessRateHealthCheck(ISuccessSubscriber successSubscriber) : IHealthCheck
{
    private const float Percent75 = 0.75f;
    private const float Percent50 = 0.5f;

    /// <summary>
    ///     Default name of the health check.
    /// </summary>
    public const string Name = "SuccessRate";

    /// <summary>
    ///     Report the service success ratio.
    /// </summary>
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var successRate = successSubscriber.SuccessRatio;
        var status = successRate switch
        {
            >= Percent75 => HealthStatus.Healthy,
            >= Percent50 => HealthStatus.Degraded,
            _ => HealthStatus.Unhealthy
        };
        return Task.FromResult(new HealthCheckResult(status));
    }
}

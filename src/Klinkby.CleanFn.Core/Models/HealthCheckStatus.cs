namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     The status of a health check.
/// </summary>
public enum HealthCheckStatus
{
    /// <summary>
    ///     The health check is passing.
    /// </summary>
    Pass = HealthStatus.Healthy,

    /// <summary>
    ///     The health check is in a warning state.
    /// </summary>
    Degraded = HealthStatus.Degraded,

    /// <summary>
    ///     The health check is failing.
    /// </summary>
    Fail = HealthStatus.Unhealthy
}

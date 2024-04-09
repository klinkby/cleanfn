namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     The status of an individual health check.
/// </summary>
/// <param name="Status">Status value</param>
public record HealthCheckItem(HealthCheckStatus Status);

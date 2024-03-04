using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     Data contract for responding service health check draft RFC-compliant.
/// </summary>
/// <seealso href="https://inadarei.github.io/rfc-healthcheck/" />
internal record Health(string Status)
{
    public const string ContentType = "application/health+json";

    public static Health FromHealthReport(HealthReport report)
    {
        return new Health(
            report.Status switch
            {
                HealthStatus.Healthy => "pass",
                HealthStatus.Degraded => "warn",
                _ => "fail"
            });
    }
}

namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     Data contract for responding service health check draft RFC-compliant.
/// </summary>
/// <seealso href="https://inadarei.github.io/rfc-healthcheck/" />
public record HealthResponse(HealthCheckStatus Status, IReadOnlyDictionary<string, HealthCheckItem> Checks)
{
    internal static HealthResponse FromHealthReport(HealthReport report)
    {
        return new HealthResponse((HealthCheckStatus)report.Status,
            report.Entries.ToDictionary(
                x => ToCamelCase(x.Key),
                x => new HealthCheckItem((HealthCheckStatus)x.Value.Status)));
    }

    private static string ToCamelCase(string value)
    {
        return value.Length switch
        {
            0 => string.Empty,
            1 => char.ToLowerInvariant(value[0]).ToString(),
            _ => char.ToLowerInvariant(value[0]) + value[1..]
        };
    }
}

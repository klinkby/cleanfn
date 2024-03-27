using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Klinkby.CleanFn.Core.Middleware;

/// <summary>
///     Add request correlation id to Application Insights telemetry.
/// </summary>
/// <param name="requestItems"></param>
internal class CorrelationTelemetryInitializer(ScopedRequestItemsAccessor? requestItems) : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        if (telemetry is ISupportProperties supportProperties
            && requestItems?.CorrelationId is { } correlationId)
        {
            supportProperties.Properties.TryAdd(KnownHeader.XCorrelationId, correlationId);
        }
    }
}

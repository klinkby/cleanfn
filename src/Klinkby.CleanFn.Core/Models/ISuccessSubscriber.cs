using Klinkby.CleanFn.Core.HealthChecks;

namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     Used by <see cref="SuccessRateHealthCheck" /> to get the success ratio from see <see cref="SuccessCounter" />.
/// </summary>
public interface ISuccessSubscriber
{
    /// <summary>
    ///     Value between 0 and 1 representing the success ratio, initially 1.
    /// </summary>
    float SuccessRatio { get; }
}

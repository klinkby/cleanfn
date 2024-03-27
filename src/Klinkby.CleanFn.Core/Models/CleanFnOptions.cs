namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     Options for the <see cref="CleanFn" />.
/// </summary>
public record CleanFnOptions
{
    /// <summary>
    ///     Map of exception types to HTTP status codes.
    /// </summary>
    public ICollection<ExceptionMap> ExceptionMap { get; init; } = new List<ExceptionMap>();
}

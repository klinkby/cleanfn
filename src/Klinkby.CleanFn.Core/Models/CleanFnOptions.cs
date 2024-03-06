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

    /// <summary>
    ///     If null (default) the server will not instruct the client to upgrade insecure requests, but handle insecure
    ///     HTTP requests.
    ///     If set, the server instruct the client to upgrades insecure requests to HTTPS on specified port.
    ///     A value of -1 means default port (443).
    /// </summary>
    public int? UpgradeInsecureRequestsToPort { get; set; }
}

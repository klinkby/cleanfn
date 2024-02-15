using System.Net;
using Klinkby.CleanFn.Core.Middleware;

namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     Options for the <see cref="ExceptionHandlerMiddleware" />.
/// </summary>
public record ExceptionHandlerOptions
{
    /// <summary>
    ///     Map of exception types to HTTP status codes.
    /// </summary>
    public ICollection<ExceptionMap> Map { get; init; } = new List<ExceptionMap>();

    /// <summary>
    ///     Fluent API for adding exception mappings.
    /// </summary>
    /// <param name="statusCode">HTTP status code</param>
    /// <typeparam name="T">Exception type</typeparam>
    /// <returns><see cref="ExceptionHandlerOptions" /> with added mapper</returns>
    public ExceptionHandlerOptions WithMap<T>(HttpStatusCode statusCode) where T : Exception
    {
        Map.Add(ExceptionMap.From<T>(statusCode));
        return this;
    }
}
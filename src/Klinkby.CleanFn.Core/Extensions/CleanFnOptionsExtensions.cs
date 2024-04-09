namespace Klinkby.CleanFn.Core.Extensions;

/// <summary>
///     Static methods relating to <see cref="CleanFnOptions" />.
/// </summary>
public static class CleanFnOptionsExtensions
{
    /// <summary>
    ///     Fluent API for adding exception mappings.
    /// </summary>
    /// <param name="map">Collection</param>
    /// <param name="statusCode">HTTP status code</param>
    /// <typeparam name="T">Exception type</typeparam>
    /// <returns><see cref="ICollection{ExceptionMap}" /> with added mapper</returns>
    public static ICollection<ExceptionMap> WithMap<T>(this ICollection<ExceptionMap> map, HttpStatusCode statusCode)
        where T : Exception
    {
        map.Add(ExceptionMap.From<T>(statusCode));
        return map;
    }
}

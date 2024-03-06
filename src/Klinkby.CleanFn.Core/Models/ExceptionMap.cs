namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     Maps an exception type to a status code.
/// </summary>
/// <param name="ExceptionType">Type of exception</param>
/// <param name="StatusCode">HTTP status code</param>
/// <seealso cref="CleanFnOptions" />
public record ExceptionMap(Type ExceptionType, HttpStatusCode StatusCode)
{
    /// <summary>
    ///     Create an <see cref="ExceptionMap" /> from a generic exception type and a status code.
    /// </summary>
    /// <param name="statusCode">HTTP status code></param>
    /// <typeparam name="T">Type of exception</typeparam>
    /// <returns>A new <see cref="ExceptionMap" /></returns>
    public static ExceptionMap From<T>(HttpStatusCode statusCode) where T : Exception
    {
        return new ExceptionMap(typeof(T), statusCode);
    }
}

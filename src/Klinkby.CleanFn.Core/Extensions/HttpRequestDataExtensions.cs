namespace Klinkby.CleanFn.Core.Extensions;

/// <summary>
///     Static methods for writing to <see cref="HttpResponseData" />.
/// </summary>
internal static class HttpRequestDataExtensions
{
    public static HttpResponseData Create404Response(
        this HttpRequestData req)
    {
        var res = req.CreateResponse();
        res.StatusCode = HttpStatusCode.NotFound;
        return res;
    }
}

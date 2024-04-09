using Klinkby.CleanFn.Core.Extensions;

namespace Klinkby.CleanFn.Core.Functions;

/// <summary>
///     If <c>swagger.json</c> or <c>swagger.yaml</c> is present in the function directory,
///     this function will return the content of that file.
/// </summary>
/// <param name="logger"></param>
public partial class SwaggerFunction(ILogger<SwaggerFunction> logger)
{
    // ReSharper disable twice InconsistentNaming
    private const string yaml = nameof(yaml);
    private const string json = nameof(json);

    /// <summary>
    /// </summary>
    /// <param name="req"></param>
    /// <param name="ext"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    [Function(nameof(SwaggerGet))]
    public async Task<HttpResponseData> SwaggerGet(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = $"swagger.{{ext:regex({yaml}|{json})}}")]
        HttpRequestData req, string ext, CancellationToken cancellationToken)
    {
        var filename = $"swagger.{ext}";
        var contentType = ext.ToLowerInvariant() switch
        {
            json => KnownContentType.ApplicationJson,
            yaml => KnownContentType.TextYaml,
            _ => throw new InvalidOperationException($"Unexpected extension: '{ext}'")
        };
        try
        {
            await using var swagger = File.OpenRead(filename);
            return await req.CreateResponse().WriteSwaggerResponse(
                swagger,
                contentType,
                3600,
                cancellationToken);
        }
        catch (FileNotFoundException)
        {
            LogFileMissing(logger, filename);
            return req.Create404Response();
        }
    }

    [LoggerMessage(LogLevel.Warning, "Swagger request failed because {Filename} was not found")]
    private static partial void LogFileMissing(ILogger logger, string filename);
}

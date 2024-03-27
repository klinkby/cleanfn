using System.Text.Json.Serialization;

namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     Source generator for response contracts <see cref="ProblemDetails" /> and <see cref="HealthResponse" />.
/// </summary>
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(HealthResponse))]
[JsonSerializable(typeof(HealthCheckItem))]
[JsonSerializable(typeof(HealthCheckStatus))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
internal partial class CleanFnSerializer : JsonSerializerContext;

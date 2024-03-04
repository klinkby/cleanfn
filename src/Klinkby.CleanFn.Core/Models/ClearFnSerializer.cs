using System.Text.Json.Serialization;

namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     Source generator for response contracts <see cref="ProblemDetails" /> and <see cref="Health" />.
/// </summary>
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(Health))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal partial class ClearFnSerializer : JsonSerializerContext;

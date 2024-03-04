using System.Text.Json.Serialization;
using SampleDomainLogic.Foo;

namespace SampleDomainLogic;

[JsonSerializable(typeof(FooGetResponse))] // <-- add all request body and response types here
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class DataContractSerializer : JsonSerializerContext;

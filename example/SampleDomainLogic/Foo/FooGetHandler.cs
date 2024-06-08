using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using Klinkby.CleanFn.Abstractions;

namespace SampleDomainLogic.Foo;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "False positive")]
[SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Test project")]
internal sealed class FooGetHandler(IHttpClientFactory client) : IRequestHandler<FooGetRequest, FooGetResponse>
{
    public async Task<FooGetResponse> Handle(FooGetRequest request, CancellationToken cancellationToken)
    {
        var httpClient = client.CreateClient("foo");
        var res = await httpClient.GetAsync(new Uri("https://httpbin.org/headers"), cancellationToken);
        res.EnsureSuccessStatusCode();
        var echoResponse = await res.Content.ReadFromJsonAsync<HttpBinHeaders>(cancellationToken);
        return new FooGetResponse { Name = $"Got a {request.Id} {echoResponse!.Headers["X-Correlation-Id"]}" };
    }
}

public record HttpBinHeaders(Dictionary<string, string> Headers);

using System.Net.Http.Json;
using Klinkby.CleanFn.Abstractions;

namespace SampleDomainLogic.Foo;

internal class FooGetHandler(IHttpClientFactory client) : IRequestHandler<FooGetRequest, FooGetResponse>
{
    public async Task<FooGetResponse> Handle(FooGetRequest request, CancellationToken cancellationToken)
    {
        var httpClient = client.CreateClient("foo");
        var res = await httpClient.GetAsync("https://httpbin.org/headers", cancellationToken);
        res.EnsureSuccessStatusCode();
        var echoResponse = await res.Content.ReadFromJsonAsync<HttpBinHeaders>(cancellationToken);
        return new FooGetResponse
        {
            Name = $"Got a {request.Id} {echoResponse!.Headers["X-Correlation-Id"]}"
        };
    }
}

public record HttpBinHeaders(Dictionary<string, string> Headers);

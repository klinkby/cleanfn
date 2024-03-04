using Klinkby.CleanFn.Abstractions;

namespace SampleDomainLogic.Foo;

internal class FooGetHandler : IRequestHandler<FooGetRequest, FooGetResponse>
{
    public Task<FooGetResponse> Handle(FooGetRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new FooGetResponse { Name = $"Jens {request.Id}" });
    }
}

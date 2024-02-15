using Klinkby.CleanFn.Abstractions;

namespace SampleDomainLogic;

public class FooGetHandler : IRequestHandler<FooGetRequest, FooGetResponse>
{
    public Task<FooGetResponse> Handle(FooGetRequest request, CancellationToken cancellationToken) =>
        Task.FromResult(new FooGetResponse { Name = $"Jens {request.Id}" });
}
namespace Klinkby.CleanFn;

/// <summary>
///     Sample implementation of a HTTP-triggered function that use a mediator to decouple
///     request validation and handling.
/// </summary>
internal class FooFunction(IMediator mediator)
{
    private const string Route = "foo";

    [Function(nameof(FooGetById))]
    public Task<FooGetResponse> FooGetById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route + "/{id}")]
        HttpRequestData req,
        int id,
        CancellationToken cancellationToken)
    {
        var query = new FooGetRequest { Id = id };
        return mediator.Send<FooGetRequest, FooGetResponse>(query, cancellationToken);
    }
}
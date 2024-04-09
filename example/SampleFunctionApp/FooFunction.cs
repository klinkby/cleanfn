using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using SampleDomainLogic.Foo;

namespace SampleFunctionApp;

/// <summary>
///     Sample implementation of a HTTP-triggered function that use a mediator to decouple
///     request validation and handling.
/// </summary>
public class FooFunction(IMediator mediator)
{
    private const string Route = "foo";

    [OpenApiOperation("greeting", "greeting", Summary = "Greetings",
        Description = "This shows a welcome message.")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FooGetResponse))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/problem+json", typeof(ProblemDetails))]
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

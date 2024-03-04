using System.ComponentModel.DataAnnotations;
using System.Net;
using Klinkby.CleanFn.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(CleanFn.Configure<DataContractSerializer>)
    .ConfigureServices(services =>
    {
        // configure exception handling
        services.AddOptions<ExceptionHandlerOptions>()
            .Configure(options => options
                .WithMap<FunctionInputConverterException>(HttpStatusCode.BadRequest)
                .WithMap<ValidationException>(HttpStatusCode.BadRequest)
                .WithMap<ArgumentException>(HttpStatusCode.BadRequest)
                .WithMap<UnauthorizedAccessException>(HttpStatusCode.Unauthorized)
                .WithMap<InvalidOperationException>(HttpStatusCode.Forbidden)
                .WithMap<HttpRequestException>(HttpStatusCode.BadGateway)
                .WithMap<TimeoutException>(HttpStatusCode.GatewayTimeout)
            );

        // add application logic
        services.AddRequestHandlers();

        // add validating mediator
        services.AddDataAnnotationsValidatingMediator();

        // support extensible health checks
        services.AddHealthChecks();
    })
    .Build();
await host.RunAsync();

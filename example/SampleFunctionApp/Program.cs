using System.ComponentModel.DataAnnotations;
using System.Net;
using Klinkby.CleanFn.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(CleanFn.ConfigureFunctionWorker<DataContractSerializer>)
    .ConfigureServices(services =>
    {
        services.AddServicesFromAssemblyOf<DataContractSerializer>();
        services.AddOptions<ExceptionHandlerOptions>()
            .Configure(options => options
                .WithMap<ValidationException>(HttpStatusCode.BadRequest)
                .WithMap<ArgumentException>(HttpStatusCode.BadRequest)
                .WithMap<UnauthorizedAccessException>(HttpStatusCode.Unauthorized)
                .WithMap<InvalidOperationException>(HttpStatusCode.Forbidden)
                .WithMap<HttpRequestException>(HttpStatusCode.ServiceUnavailable)
                .WithMap<HttpRequestException>(HttpStatusCode.ServiceUnavailable)
                .WithMap<TimeoutException>(HttpStatusCode.GatewayTimeout)
                );
    })
    .Build();
await host.RunAsync();
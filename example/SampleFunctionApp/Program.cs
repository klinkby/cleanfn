using System.ComponentModel.DataAnnotations;
using System.Net;
using Klinkby.CleanFn.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(CleanFn.Configure<DataContractSerializer>)
    .ConfigureServices(services =>
    {
        // add application logic
        services.AddRequestHandlers();

        // add http client factory (move to infrastructure)  
        services.AddHttpClient();
        
        // configure exception handling
        services.AddOptions<CleanFnOptions>()
            .Configure(options =>
            {
               // options.UpgradeInsecureRequestsToPort = 7072;
                options.ExceptionMap
                    .WithMap<FunctionInputConverterException>(HttpStatusCode.BadRequest)
                    .WithMap<ValidationException>(HttpStatusCode.BadRequest)
                    .WithMap<ArgumentException>(HttpStatusCode.BadRequest)
                    .WithMap<UnauthorizedAccessException>(HttpStatusCode.Unauthorized)
                    .WithMap<InvalidOperationException>(HttpStatusCode.Forbidden)
                    .WithMap<HttpRequestException>(HttpStatusCode.BadGateway)
                    .WithMap<TimeoutException>(HttpStatusCode.GatewayTimeout);
            });

        // add validating mediator
        services.AddDataAnnotationsValidatingMediator();

        // support extensible health checks
        services.AddHealthChecks();
    })
    .Build();
await host.RunAsync();

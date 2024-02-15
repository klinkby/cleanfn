using Microsoft.Extensions.Hosting;
using SampleDomainLogic;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(CleanFn.ConfigureFunctionWorker<DataContractSerializer>)
    .ConfigureServices(CleanFn.AddServicesFromAssemblyOf<DataContractSerializer>)
    .Build();
await host.RunAsync();
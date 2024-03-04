using Klinkby.CleanFn.Abstractions;
using SampleDomainLogic.Foo;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequestHandlers(this IServiceCollection services)
    {
        services.AddTransient<IRequestHandler<FooGetRequest, FooGetResponse>, FooGetHandler>();
        return services;
    }
}

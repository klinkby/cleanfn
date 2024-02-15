using System.Reflection;
using Klinkby.CleanFn.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Klinkby.CleanFn.Core.Extensions;

/// <summary>
///     Static methods for registering services.
/// </summary>
internal static class ServiceCollectionExtensions
{
    public static void AddRequestHandlers(this IServiceCollection services, params Assembly[] assemblies)
    {
        // Use https://github.com/khellang/Scrutor to scan assemblies for IRequestHandlerBase implementations
        services.Scan(selector => selector
            .FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IRequestHandlerBase)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}
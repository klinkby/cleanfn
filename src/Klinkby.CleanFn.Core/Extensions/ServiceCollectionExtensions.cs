using System.ComponentModel.DataAnnotations;
using Klinkby.CleanFn.Core;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Static methods for registering services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Add basic non-validating mediator to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <returns>The <see cref="IServiceCollection" /></returns>
    public static IServiceCollection AddBasicMediator(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator>();
        return services;
    }

    /// <summary>
    ///     Add a mediator that use declarative data annotations <see cref="Validator" /> to assert the requests.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <returns>The <see cref="IServiceCollection" /></returns>
    public static IServiceCollection AddDataAnnotationsValidatingMediator(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, DataAnnotationsValidatorMediator>();
        return services;
    }

    internal static IServiceCollection AddApplicationInsights(this IServiceCollection services)
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        return services;
    }
}

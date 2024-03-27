using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Azure.Core.Serialization;
using Klinkby.CleanFn.Abstractions;
using Klinkby.CleanFn.Core.Handlers;
using Klinkby.CleanFn.Core.HttpClientFilters;
using Klinkby.CleanFn.Core.Middleware;
using Klinkby.CleanFn.Core.Models;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;

namespace Klinkby.CleanFn.Core;

/// <summary>
///     Static methods for bootstrapping CleanFn.
/// </summary>
public static class CleanFn
{
    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="builder"></param>
    /// <typeparam name="TSerializer">Source generated serializer for HTTP request body and response DTOs.</typeparam>
    public static void Configure<TSerializer>(HostBuilderContext context, IFunctionsWorkerApplicationBuilder builder)
        where TSerializer : IJsonTypeInfoResolver, new()
    {
        builder.UseWhen<CleanFnMiddleware>(IsHttpTrigger);

        var services = builder.Services;
        services.AddTransient<IRequestHandler<HealthRequest, HealthResponse>, HealthRequestHandler>();
        services.AddScoped<ScopedRequestItemsAccessor>();
        services.AddScoped<IHttpMessageHandlerBuilderFilter, AddCorrelationHttpMessageHandlerBuilderFilter>();
        services.Configure<WorkerOptions>(options =>
            // require serializer source generation
            options.Serializer = new JsonObjectSerializer(
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                    TypeInfoResolver = JsonTypeInfoResolver.Combine(
                        new CleanFnSerializer(),
                        new TSerializer())
                }
            )
        );
        if (!IsApplicationInsightsEnabled(context.Configuration))
        {
            return;
        }

        services.AddApplicationInsights();
        services.AddScoped<ITelemetryInitializer, CorrelationTelemetryInitializer>();
    }

    private static bool IsApplicationInsightsEnabled(IConfiguration configuration)
    {
        var appInsights = configuration.GetValue<string?>("APPINSIGHTS_INSTRUMENTATIONKEY");
        return !string.IsNullOrEmpty(appInsights);
    }

    private static bool IsHttpTrigger(FunctionContext ctx)
    {
        return ctx
            .FunctionDefinition
            .InputBindings
            .Values
            .First(a => a.Type.EndsWith("Trigger"))
            .Type == "httpTrigger";
    }
}

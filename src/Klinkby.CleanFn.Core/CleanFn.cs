using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Azure.Core.Serialization;
using Klinkby.CleanFn.Core.Middleware;
using Klinkby.CleanFn.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Klinkby.CleanFn.Core;

/// <summary>
///     Static methods for bootstrapping CleanFn.
/// </summary>
public static class CleanFn
{
    /// <summary>
    /// </summary>
    /// <param name="_"></param>
    /// <param name="builder"></param>
    /// <typeparam name="TSerializer">Source generated serializer for HTTP request body and response DTOs.</typeparam>
    public static void Configure<TSerializer>(HostBuilderContext _, IFunctionsWorkerApplicationBuilder builder)
        where TSerializer : IJsonTypeInfoResolver, new()
    {
        builder.UseWhen<ExceptionHandlerMiddleware>(IsHttpTrigger);
        builder.UseWhen<CorrelationMiddleware>(IsHttpTrigger);

        var services = builder.Services;
        services.Configure<WorkerOptions>(options =>
            // require serializer source generation
            options.Serializer = new JsonObjectSerializer(
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    TypeInfoResolver = JsonTypeInfoResolver.Combine(
                        new ClearFnSerializer(),
                        new TSerializer())
                }
            )
        );
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

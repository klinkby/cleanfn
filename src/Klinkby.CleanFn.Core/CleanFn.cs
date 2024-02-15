using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Azure.Core.Serialization;
using Klinkby.CleanFn.Core.Extensions;
using Klinkby.CleanFn.Core.Middleware;
using Klinkby.CleanFn.Core.Models;
using Klinkby.CleanFn.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Klinkby.CleanFn.Core;

/// <summary>
///    Static methods for bootstrapping CleanFn.
/// </summary>
public static class CleanFn
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_"></param>
    /// <param name="builder"></param>
    /// <typeparam name="T"></typeparam>
    public static void ConfigureFunctionWorker<T>(HostBuilderContext _, IFunctionsWorkerApplicationBuilder builder) 
        where T : IJsonTypeInfoResolver, new()
    {
        builder.Services.Configure<WorkerOptions>(options =>
            // require serializer source generation
            options.Serializer = new JsonObjectSerializer(
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    TypeInfoResolver = JsonTypeInfoResolver.Combine(
                        new ClearFnSerializer(),
                        new T())
                })
        );
        builder.UseWhen<ExceptionHandlerMiddleware>(IsHttpTrigger);
        builder.UseWhen<CorrelationMiddleware>(IsHttpTrigger);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="T"></typeparam>
    public static void AddServicesFromAssemblyOf<T>(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddSingleton<IMediator, DataAnnotationsValidatorMediator>();
        services.AddRequestHandlers(typeof(T).Assembly);
    }

    private static bool IsHttpTrigger(FunctionContext ctx) =>
        ctx
            .FunctionDefinition
            .InputBindings
            .Values
            .First(a => a.Type.EndsWith("Trigger"))
            .Type == "httpTrigger";
}
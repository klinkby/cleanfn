using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Klinkby.CleanFn.Core.Middleware;

/// <summary>
///     Interceptor for fetching or generating, logging and responding correlation id.
/// </summary>
/// <remarks>Attach the correlationId to any outgoing requests.</remarks>
/// <seealso href="https://microsoft.github.io/code-with-engineering-playbook/observability/correlation-id/" />
internal partial class CorrelationInterceptor(ILogger logger, IServiceProvider services)
    : IFunctionsWorkerInterceptor
{
    public ValueTask<bool> OnExecutingAsync(FunctionContext context, HttpRequestData request,
        CancellationToken cancellationToken)
    {
        var correlationId = request.Headers.TryGetValues(KnownHeader.XCorrelationId, out var requestCorrelationId)
            ? requestCorrelationId.First()
            : context.InvocationId;
        context.Items.TryAdd(KnownHeader.XCorrelationId, correlationId);
        services.GetRequiredService<ScopedRequestItemsAccessor>().RequestItems = context.Items;
        LogCorrelationId(logger, correlationId);
        return ValueTask.FromResult(true);
    }

    public ValueTask OnExecutedAsync(FunctionContext context, HttpRequestData request, HttpResponseData response,
        Exception? pipelineException, CancellationToken cancellationToken)
    {
        var headers = response.Headers;
        headers.TryAddWithoutValidation(KnownHeader.XCorrelationId,
            context.Items[KnownHeader.XCorrelationId] as string);
        return ValueTask.CompletedTask;
    }

    [LoggerMessage(LogLevel.Information, KnownHeader.XCorrelationId + " {CorrelationId}")]
    static partial void LogCorrelationId(ILogger logger, string correlationId);
}

using Microsoft.Azure.Functions.Worker.Http;

namespace Klinkby.CleanFn.Core.Middleware;

internal interface IFunctionsWorkerInterceptor
{
    ValueTask<bool> OnExecutingAsync(FunctionContext context, HttpRequestData request,
        CancellationToken cancellationToken);

    ValueTask OnExecutedAsync(FunctionContext context, HttpRequestData request, HttpResponseData response,
        Exception? pipelineException, CancellationToken cancellationToken);
}

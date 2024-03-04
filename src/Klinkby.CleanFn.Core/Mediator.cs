using Klinkby.CleanFn.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Klinkby.CleanFn.Core;

/// <summary>
///     Basic mediator implementation dispatches requests (commands or queries) to handlers,
///     and handles and logging. Override for validation.
/// </summary>
/// <remarks>Override for validation</remarks>
public partial class Mediator(IServiceProvider services, ILogger<Mediator> logger) : IMediator
{
    /// <inheritdoc />
    public Task Send<TRequest>(TRequest command, CancellationToken cancellationToken)
        where TRequest : IRequest
    {
        var x = services.GetService<IRequestHandler<TRequest>>();
        if (x == null)
        {
            throw new NotImplementedException($"No {typeof(TRequest).Name} handler");
        }

        Validate(command);
        LogHandleRequest(logger, typeof(TRequest).Name);
        return x.Handle(command, cancellationToken);
    }

    /// <inheritdoc />
    public Task<TResponse> Send<TRequest, TResponse>(TRequest query, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        var x = services.GetService<IRequestHandler<TRequest, TResponse>>();
        if (x == null)
        {
            throw new NotImplementedException($"No {typeof(TRequest).Name} handler");
        }

        Validate(query);
        LogHandleRequest(logger, typeof(TRequest).Name);
        return x.Handle(query, cancellationToken);
    }

    /// <summary>
    ///     Override to add validation of the request object before dispatching to the handler.
    /// </summary>
    /// <param name="request"></param>
    protected virtual void Validate(IRequestBase request)
    {
    }

    [LoggerMessage(LogLevel.Information, "Handle {Request}")]
    private static partial void LogHandleRequest(ILogger logger, string request);
}

using Klinkby.CleanFn.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Klinkby.CleanFn.Core.Services;

/// <summary>
///     Basic mediator implementation dispatches requests (commands or queries) to handlers,
///     and handles and logging.
/// </summary>
/// <seealso cref="DataAnnotationsValidatorMediator" />
/// <remarks>Override for validation</remarks>
internal partial class Mediator(IServiceProvider services, ILogger<Mediator> logger) : IMediator
{
    public Task Send<TRequest>(TRequest command, CancellationToken cancellationToken)
        where TRequest : IRequest
    {
        var x = services.GetService<IRequestHandler<TRequest>>();
        if (x == null) throw new NotImplementedException($"No {typeof(TRequest).Name} handler");
        Validate(command);
        LogHandleRequest(logger, typeof(TRequest).Name);
        return x.Handle(command, cancellationToken);
    }

    public Task<TResponse> Send<TRequest, TResponse>(TRequest query, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        var x = services.GetService<IRequestHandler<TRequest, TResponse>>();
        if (x == null) throw new NotImplementedException($"No {typeof(TRequest).Name} handler");
        Validate(query);
        LogHandleRequest(logger, typeof(TRequest).Name);
        return x.Handle(query, cancellationToken);
    }

    protected virtual void Validate(object request)
    {
    }

    [LoggerMessage(LogLevel.Information, "Handle {Request}")]
    private static partial void LogHandleRequest(ILogger logger, string request);
}
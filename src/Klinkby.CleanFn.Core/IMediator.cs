namespace Klinkby.CleanFn.Core;

/// <summary>
///     Define the core mediator for dispatching requests to handlers.
/// </summary>
public interface IMediator
{
    /// <summary>
    ///     Dispatch a command to a handler.
    /// </summary>
    /// <param name="command">Request command parameters</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation.</param>
    /// <typeparam name="TRequest">Type of request command parameters</typeparam>
    /// <returns>Awaitable task</returns>
    Task Send<TRequest>(TRequest command, CancellationToken cancellationToken) where TRequest : IRequest;

    /// <summary>
    ///     Dispatch a query to a handler that generate a response.
    /// </summary>
    /// <remarks>TResponse must have a registered source generated serializer</remarks>
    /// <param name="query">Request query parameters</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation.</param>
    /// <typeparam name="TRequest">Type of request query parameters</typeparam>
    /// <typeparam name="TResponse">Type of response contract</typeparam>
    /// <returns>Awaitable task with response</returns>
    Task<TResponse> Send<TRequest, TResponse>(TRequest query, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>;
}

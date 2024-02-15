namespace Klinkby.CleanFn.Abstractions;

/// <summary>
///     Base interface for request parameters.
/// </summary>
/// <remarks>
///     Should not be implemented directly,
///     use <see cref="IRequest{TResponse}" />
///     or <see cref="IRequest" /> instead.
/// </remarks>
public interface IRequestHandlerBase;

/// <summary>
///     Handler for command requests that does not return a value.
/// </summary>
/// <typeparam name="TRequest">The command parameters type.</typeparam>
public interface IRequestHandler<in TRequest> : IRequestHandlerBase
    where TRequest : IRequest
{
    /// <summary>
    ///     Handle the command request that does not return a value.
    /// </summary>
    /// <param name="command">The command parameters.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation.</param>
    /// <returns>An awaitable <see cref="Task" /></returns>
    Task Handle(TRequest command, CancellationToken cancellationToken);
}

/// <summary>
///     Handler for query requests that returns a value.
/// </summary>
/// <typeparam name="TRequest">Command parameters type</typeparam>
/// <typeparam name="TResponse">Response value type</typeparam>
public interface IRequestHandler<in TRequest, TResponse> : IRequestHandlerBase
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    ///     Handle the query request that return a value.
    /// </summary>
    /// <param name="query">The query parameters.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation.</param>
    /// <returns>An awaitable <see cref="Task{TResponse}" /> with the response value.</returns>
    Task<TResponse> Handle(TRequest query, CancellationToken cancellationToken);
}
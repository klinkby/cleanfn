namespace Klinkby.CleanFn.Abstractions;

/// <summary>
///     Base interface for request parameters.
/// </summary>
public interface IRequestBase;

/// <summary>
///     Define a command request that does not return a value.
/// </summary>
public interface IRequest : IRequestBase;

/// <summary>
///     Define a query request that returns a response value.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
// ReSharper disable once UnusedTypeParameter
public interface IRequest<out TResponse> : IRequestBase;
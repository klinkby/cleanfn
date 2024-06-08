using System.Diagnostics.CodeAnalysis;

namespace Klinkby.CleanFn.Abstractions;

/// <summary>
///     Base interface for request parameters.
/// </summary>
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Base for IRequests types")]
public interface IRequestBase;

/// <summary>
///     Define a command request that does not return a value.
/// </summary>
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Marks IRequest types without a return value")]
public interface IRequest : IRequestBase;

/// <summary>
///     Define a query request that returns a response value.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
// ReSharper disable once UnusedTypeParameter
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Marks IRequest types with a return value")]
public interface IRequest<out TResponse> : IRequestBase;

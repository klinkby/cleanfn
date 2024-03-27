using Klinkby.CleanFn.Core.HttpClientFilters;

namespace Klinkby.CleanFn.Core.Middleware;

/// <summary>
///     Scoped service for passing http context request items in <see cref="CorrelationInterceptor" /> to
///     <see cref="HttpClient" /> message handler <see cref="AddCorrelationHttpMessageHandlerBuilderFilter" />.
/// </summary>
internal class ScopedRequestItemsAccessor
{
    public IDictionary<object, object>? RequestItems { get; set; }

    public string? CorrelationId => RequestItems?[KnownHeader.XCorrelationId] as string;
}

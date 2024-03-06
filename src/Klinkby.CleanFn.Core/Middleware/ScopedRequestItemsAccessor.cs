using Klinkby.CleanFn.Core.HttpClientFilters;
using Klinkby.CleanFn.Core.Middleware;

namespace Klinkby.CleanFn.Core;

/// <summary>
///     Scoped service for passing http context request items in <see cref="CorrelationInterceptor"/> to <see cref="HttpClient"/> message handler <see cref="AddCorrelationHttpMessageHandlerBuilderFilter"/>.
/// </summary>
internal class ScopedRequestItemsAccessor
{
    public IDictionary<object, object>? RequestItems { get; set; }
}

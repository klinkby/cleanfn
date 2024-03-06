﻿using Klinkby.CleanFn.Core.Middleware;
using Microsoft.Extensions.Http;

namespace Klinkby.CleanFn.Core.HttpClientFilters;

/// <summary>
///     Adds correlation id to outgoing HTTP requests.
/// </summary>
/// <param name="itemsAccessor">Http context request item values</param>
internal class AddCorrelationHttpMessageHandlerBuilderFilter(ScopedRequestItemsAccessor? itemsAccessor)
    : IHttpMessageHandlerBuilderFilter
{
    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        return builder =>
        {
            var items = itemsAccessor?.RequestItems;
            if (null != items 
                && items.TryGetValue(CorrelationInterceptor.CorrelationIdHeader, out var value))
            {
                builder.AdditionalHandlers.Add(
                    new AddRequestHeaderHandler(CorrelationInterceptor.CorrelationIdHeader, value as string));
            }

            next(builder);
        };
    }

    /// <summary>
    /// Delegating handler for setting a header on the HTTP request
    /// </summary>
    private sealed class AddRequestHeaderHandler(string headerName, string? value) : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Headers.TryAddWithoutValidation(headerName, value);
            return base.SendAsync(request, cancellationToken);
        }
    }
}

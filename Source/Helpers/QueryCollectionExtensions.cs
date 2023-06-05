// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Primitives;

namespace Aksio.IngressMiddleware.Helpers;

/// <summary>
/// Extension methods for <see cref="IDictionary{TKey,TValue}"/>.
/// </summary>
public static class QueryCollectionExtensions
{
    /// <summary>
    /// Converts a <see cref="IDictionary{TKey,TValue}"/> to a query string.
    /// </summary>
    /// <param name="query">Dictionary representing the query to convert.</param>
    /// <returns>A querystring.</returns>
    public static string ToQueryString(this IDictionary<string, StringValues> query) =>
        string.Join("&", query.Select(_ => string.Format($"{_.Key}={_.Value}")));
}
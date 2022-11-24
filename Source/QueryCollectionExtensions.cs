// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Primitives;

namespace Aksio.IngressMiddleware;

public static class QueryCollectionExtensions
{
    public static IQueryCollection ChangeScope(this IQueryCollection query, string scope) => 
        new QueryCollection(
         query.Select(_ => _.Key switch
            {
                "scope" => new(_.Key, scope),
                _ => _
            }
        ).ToDictionary(_ => _.Key, _ => _.Value));

    public static IQueryCollection SetRedirectUri(this IQueryCollection query, string redirectUri) =>
        new QueryCollection(
         query.Select(_ => _.Key switch
            {
                "redirect_uri" => new(_.Key, Uri.EscapeDataString(redirectUri)),
                _ => _
            }
        ).ToDictionary(_ => _.Key, _ => _.Value));

    public static string ToQueryString(this IDictionary<string, StringValues> query) =>
        string.Join("&", query.Select(_ => string.Format($"{_.Key}={_.Value}")));
}
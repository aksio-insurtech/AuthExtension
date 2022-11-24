// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Primitives;

namespace Aksio.IngressMiddleware;

public static class QueryCollectionExtensions
{
    public static IQueryCollection WithScope(this IQueryCollection query, string scope) =>
        new QueryCollection(
         query.Select(_ => _.Key switch
            {
                "scope" => new(_.Key, scope),
                _ => _
            }
        ).ToDictionary(_ => _.Key, _ => _.Value));

    public static IQueryCollection WithResponseType(this IQueryCollection query, string responseType) =>
        new QueryCollection(
         query.Select(_ => _.Key switch
            {
                "response_type" => new(_.Key, responseType),
                _ => _
            }
        ).ToDictionary(_ => _.Key, _ => _.Value));

    public static IQueryCollection WithRedirectUri(this IQueryCollection query, string redirectUri) =>
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
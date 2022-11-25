// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Primitives;

namespace Aksio.IngressMiddleware;

public static class QueryCollectionExtensions
{
    public static string ToQueryString(this IDictionary<string, StringValues> query) =>
        string.Join("&", query.Select(_ => string.Format($"{_.Key}={_.Value}")));
}
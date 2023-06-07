// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Helpers;

/// <summary>
/// Extension methods for <see cref="HttpRequest"/>.
/// </summary>
public static class HttpRequestExtensions
{
    /// <summary>
    /// Gets the original URI from the request. Based on the x-original-uri header.
    /// </summary>
    /// <param name="request"><see cref="HttpRequest"/> instance.</param>
    /// <returns>Original URI.</returns>
    public static Uri GetOriginalUri(this HttpRequest request) => new(request.Headers[Headers.OriginalUri].ToString());

    /// <summary>
    /// Gets whether or not the request is an impersonation route.
    /// </summary>
    /// <param name="request"><see cref="HttpRequest"/> instance.</param>
    /// <returns>True if it is, false if not.</returns>
    public static bool IsImpersonateRoute(this HttpRequest request) =>
        request.GetOriginalUri().PathAndQuery.StartsWith(WellKnownPaths.Impersonation, StringComparison.InvariantCultureIgnoreCase);
}
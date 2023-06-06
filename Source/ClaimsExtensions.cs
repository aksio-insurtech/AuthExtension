// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware;

/// <summary>
/// Extension methods for working with claims.
/// </summary>
public static class ClaimsExtensions
{
    /// <summary>
    /// Get groups from claims.
    /// </summary>
    /// <param name="claims">Claims to get from.</param>
    /// <returns>Collection of groups, if any.</returns>
    public static IEnumerable<string> GetGroups(this IEnumerable<Claim> claims)
    {
        return claims.Where(_ => _.Type == "groups").Select(_ => _.Value);
    }
}
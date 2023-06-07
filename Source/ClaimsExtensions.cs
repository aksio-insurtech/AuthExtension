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

    /// <summary>
    /// Convert from collection of <see cref="RawClaim"/> to collection of <see cref="Claim"/>.
    /// </summary>
    /// <param name="claims">Raw claims to convert from.</param>
    /// <returns>Collection of <see cref="Claim"/>.</returns>
    public static IEnumerable<Claim> ToClaims(this IEnumerable<RawClaim> claims) =>
         claims.Select(_ => new Claim(_.Type, _.Value)).ToArray();

    /// <summary>
    /// Convert from collection of <see cref="Claim"/> to collection of <see cref="RawClaim"/>.
    /// </summary>
    /// <param name="claims">Raw claims to convert from.</param>
    /// <returns>Collection of <see cref="Claim"/>.</returns>
    public static IEnumerable<RawClaim> ToRawClaims(this IEnumerable<Claim> claims) =>
         claims.Select(_ => new RawClaim(_.Type, _.Value)).ToArray();
}
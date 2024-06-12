// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IdentityModel.Tokens.Jwt;

namespace Aksio.IngressMiddleware.Security;

/// <summary>
/// Extensions for <see cref="JwtSecurityToken"/>.
/// </summary>
public static class JwtSecurityTokenExtensions
{
    /// <summary>
    /// Converts a <see cref="JwtSecurityToken"/> to a <see cref="ClientPrincipal"/>.
    /// </summary>
    /// <param name="token"><see cref="JwtSecurityToken"/> to convert.</param>
    /// <param name="identityProvider">The identity provider.</param>
    /// <returns>Converted <see cref="ClientPrincipal"/>.</returns>
    public static ClientPrincipal ToClientPrincipal(this JwtSecurityToken token, string identityProvider)
        => new(
            identityProvider,
            token.Issuer,
            token.Subject,
            token.Subject,
            token.Audiences,
            token.Claims.Select(_ => new Claim(_.Type, _.Value)));
}

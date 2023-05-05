// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IdentityModel.Tokens.Jwt;

namespace Aksio.IngressMiddleware;

public static class JwtSecurityTokenExtensions
{
    public static ClientPrincipal ToClientPrincipal(this JwtSecurityToken token)
        => new ClientPrincipal(
            token.Issuer,
            token.Subject,
            token.Subject,
            token.Audiences,
            token.Claims.Select(_ => new Claim(_.Type, _.Value)));
}

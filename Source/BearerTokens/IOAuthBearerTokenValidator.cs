// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IdentityModel.Tokens.Jwt;

namespace Aksio.IngressMiddleware.BearerTokens;

/// <summary>
/// OAuth/JWT token validator.
/// This is responsible for fetching and updating the authority keys, and validating the client token.
/// </summary>
public interface IOAuthBearerTokenValidator
{
    /// <summary>
    /// Refresh the json web keys, if necessary.
    /// </summary>
    /// <returns>True if ok, false on error.</returns>
    Task<bool> RefreshJwks();

    /// <summary>
    /// Validates the token.
    /// </summary>
    /// <param name="token">Bearer token from client.</param>
    /// <param name="jwtToken">The output token.</param>
    /// <returns>True if token is valid, false if not.</returns>
    bool ValidateToken(string token, out JwtSecurityToken? jwtToken);
}
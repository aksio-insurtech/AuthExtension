// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json;
using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware;

/// <summary>
/// Represents a client principal based on the Microsoft Identity Platform.
/// </summary>
/// <param name="IdentityProvider">A string representing the identity provider, e.g. aad.</param>
/// <param name="UserId">The unique user identifier from the identity provider.</param>
/// <param name="UserDetails">The user details, typically the user name.</param>
/// <param name="UserRoles">Collection of roles for the user.</param>
/// <param name="Claims">Collection of all the claims for the user.</param>
public record ClientPrincipal(string IdentityProvider, string UserId, string UserDetails, IEnumerable<string> UserRoles, IEnumerable<Claim> Claims)
{
    /// <summary>
    /// Represents an empty <see cref="ClientPrincipal"/>.
    /// </summary>
    public static readonly ClientPrincipal Empty = new(string.Empty, string.Empty, string.Empty, Array.Empty<string>(), Array.Empty<Claim>());

    /// <summary>
    /// Convert from a base64 encoded string to a <see cref="ClientPrincipal"/>.
    /// </summary>
    /// <param name="userId">The user identifier to convert from.</param>
    /// <param name="base64">The base64 string representation.</param>
    /// <returns>A new <see cref="ClientPrincipal"/> instance.</returns>
    public static ClientPrincipal FromBase64(string userId, string base64)
    {
        var json = Convert.FromBase64String(base64);
        var jsonText = Encoding.UTF8.GetString(json);
        var rawPrincipal = JsonSerializer.Deserialize<RawClientPrincipal>(jsonText, Globals.JsonSerializerOptions)!;

        var name = string.IsNullOrEmpty(rawPrincipal.name_typ) ? string.Empty : rawPrincipal.claims.FirstOrDefault(_ => _.Type == rawPrincipal.name_typ)?.Value ?? string.Empty;
        var roles = string.IsNullOrEmpty(rawPrincipal.role_typ) ? Enumerable.Empty<string>() : rawPrincipal.claims.Where(_ => _.Type == rawPrincipal.role_typ).Select(_ => _.Value).ToArray();
        var claims = rawPrincipal.claims?.ToClaims() ?? Enumerable.Empty<Claim>();
        return new ClientPrincipal(rawPrincipal.auth_typ, userId ?? string.Empty, name, roles, claims);
    }

    /// <summary>
    /// Convert to base64 encoded string.
    /// </summary>
    /// <returns>A base64 encoded string.</returns>
    public string ToBase64()
    {
        var rawPrincipal = new RawClientPrincipal(IdentityProvider, string.Empty, string.Empty, Claims.ToRawClaims());
        var json = JsonSerializer.SerializeToUtf8Bytes(rawPrincipal, Globals.JsonSerializerOptions);
        return Convert.ToBase64String(json);
    }

    /// <summary>
    /// Convert to a <see cref="RawClientPrincipal"/>.
    /// </summary>
    /// <returns>Converted <see cref="RawClientPrincipal"/>.</returns>
    public RawClientPrincipal ToRawClientPrincipal()
    {
        var nameType = Claims.FirstOrDefault(_ => _.Value == UserDetails)?.Type ?? string.Empty;
        var roleType = string.Empty;
        if (UserRoles.Any())
        {
            roleType = Claims.FirstOrDefault(_ => _.Value == UserRoles.First())?.Type ?? string.Empty;
        }
        return new RawClientPrincipal(IdentityProvider, nameType, roleType, Claims.ToRawClaims());
    }
}

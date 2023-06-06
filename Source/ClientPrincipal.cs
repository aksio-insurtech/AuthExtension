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
    record RawClientPrincipal(string auth_typ, string name_typ, string role_typ, IEnumerable<Claim> claims);

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

        var name = rawPrincipal.claims.FirstOrDefault(_ => _.Type == rawPrincipal.name_typ)?.Value ?? string.Empty;
        var roles = rawPrincipal.claims.Where(_ => _.Type == rawPrincipal.role_typ).Select(_ => _.Value).ToArray();
        return new ClientPrincipal(rawPrincipal.auth_typ, userId, name, roles, rawPrincipal.claims);
    }
}

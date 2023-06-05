// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json;
using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware;

public record ClientPrincipal(string IdentityProvider, string UserId, string UserDetails, IEnumerable<string> UserRoles, IEnumerable<Claim> Claims)
{
    record RawClientPrincipal(string auth_typ, string name_typ, string role_typ, IEnumerable<Claim> claims);

    public static ClientPrincipal FromBase64(string base64)
    {
        var json = Convert.FromBase64String(base64);
        var jsonText = Encoding.UTF8.GetString(json);
        var rawPrincipal = JsonSerializer.Deserialize<RawClientPrincipal>(jsonText, Globals.JsonSerializerOptions)!;

        var name = rawPrincipal.claims.FirstOrDefault(_ => _.Type == rawPrincipal.name_typ)?.Value ?? string.Empty;
        var roles = rawPrincipal.claims.Where(_ => _.Type == rawPrincipal.role_typ).Select(_ => _.Value).ToArray();
        return new ClientPrincipal(rawPrincipal.auth_typ, string.Empty, name, roles, rawPrincipal.claims);
    }
}

// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

namespace Aksio.IngressMiddleware.integrationtests.routeSourceIdentifierResolver.given;

public class route_source_specification : Specification
{
    protected void BuildAndSetPrincipalWithTenantClaim(
        HttpRequestMessage requestMessage,
        string claimedTenantId,
        string authAudience,
        params string[] roles)
    {
        var claims = new List<RawClaim>()
        {
            new(ClaimsSourceIdentifier.EntraIdTenantIdClaim, claimedTenantId),
        };
        if (!string.IsNullOrEmpty(authAudience))
        {
            claims.Add(new("aud", authAudience));
        }

        claims.AddRange(roles.Select(r => new RawClaim("roles", r)));

        var principal = new RawClientPrincipal("testprovider", "testuser", "userdetails", claims);
        var jsonPrincipal = JsonSerializer.Serialize(principal, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        requestMessage.Headers.Add(Headers.Principal, Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonPrincipal)));
    }
}
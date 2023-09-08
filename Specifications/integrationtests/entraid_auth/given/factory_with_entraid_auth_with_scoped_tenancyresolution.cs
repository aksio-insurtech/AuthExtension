// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Tenancy;

namespace Aksio.IngressMiddleware.integrationtests.entraid_auth.given;

public class factory_with_entraid_auth_with_scoped_tenancyresolution : Specification
{
    protected IngressWebApplicationFactory IngressFactory;
    protected HttpClient IngressClient;
    protected Config IngressConfig;

    void Establish()
    {
        IngressConfig = new()
        {
            Tenants = new()
            {
                {
                    Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    new() { SourceIdentifiers = new[] { "sourceid_1", "sourceid_2" } }
                },
                {
                    Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    new() { SourceIdentifiers = new[] { "sourceid_3", "sourceid_4" } }
                }
            },
            TenantResolution = new()
            {
                Strategy = TenantSourceIdentifierResolverType.Claim
            },
            IdentityDetailsUrl = string.Empty
        };

        IngressFactory = new()
        {
            Config = IngressConfig
        };

        IngressClient = IngressFactory.CreateClient();
    }

    /// <summary>
    /// Helper to set the claimed tenantid for the request.
    /// </summary>
    /// <param name="requestMessage">The request.</param>
    /// <param name="claimedTenantId">Tenant id to claim.</param>
    protected void BuildAndSetPrincipalWithTenantClaim(HttpRequestMessage requestMessage, string claimedTenantId)
    {
        var principal = new RawClientPrincipal(
            "testprovider",
            "testuser",
            "userdetails",
            new[] { new RawClaim(ClaimsSourceIdentifierResolver.TenantIdClaim, claimedTenantId) });

        var jsonPrincipal = JsonSerializer.Serialize(principal, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        requestMessage.Headers.Add(Headers.Principal, Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonPrincipal)));
    }
}
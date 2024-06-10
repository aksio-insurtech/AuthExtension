// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

namespace Aksio.IngressMiddleware.integrationtests.role_authorization.given;

public class factory_with_host_resolution_and_role_auth_with_scoped_tenancyresolution : Specification
{
    protected IngressWebApplicationFactory IngressFactory;
    protected HttpClient IngressClient;
    protected Config IngressConfig;
    protected Dictionary<string, AuthorizationAudienceConfig> AcceptedRolesPrAudience;
    protected string AudienceWithRoles = "audienceWithRoles";
    protected string AudienceWithNoAuthRequired = "audienceWithNoAuth";
    protected string EntraId1 = "entraid1";

    void Establish()
    {
        AcceptedRolesPrAudience = new()
        {
            {
                AudienceWithRoles, new() { Roles = new List<string> { "testrole", "secondrole", "otherrole" } }
            },
            {
                AudienceWithNoAuthRequired, new() { NoAuthorizationRequired = true }
            }
        };

        IngressConfig = new()
        {
            Tenants = new()
            {
                {
                    Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    new()
                    {
                        SourceIdentifiers = new[] { "0001" },
                        EntraIdTenants = new[] { EntraId1 }
                    }
                },
                {
                    Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    new()
                    {
                        SourceIdentifiers = new[] { "0002" }
                    }
                },
            },
            TenantResolutions = new[]
            {
                new TenantResolutionConfig()
                {
                    Strategy = TenantSourceIdentifierResolverType.Host,
                    Options = JsonSerializer.Deserialize<JsonObject>(
                        JsonSerializer.Serialize(
                            new RequestHostSourceIdentifierOptions()
                            {
                                Hostnames = new Dictionary<string, string>()
                                {
                                    { "host0001", "0001" },
                                    { "host0002", "0002" }
                                }
                            }))
                },
                new TenantResolutionConfig()
                {
                    Strategy = TenantSourceIdentifierResolverType.Claim
                }
            },
            IdentityDetailsUrl = string.Empty,
            Authorization = AcceptedRolesPrAudience
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
    protected void BuildAndSetPrincipalWithTenantClaim(
        HttpRequestMessage requestMessage,
        string claimedTenantId,
        string authAudience,
        string preferredUsername,
        params string[] roles)
    {
        var claims = new List<RawClaim>
        {
            new(ClaimsSourceIdentifier.EntraIdTenantIdClaim, claimedTenantId),
            new("preferred_username", preferredUsername)
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
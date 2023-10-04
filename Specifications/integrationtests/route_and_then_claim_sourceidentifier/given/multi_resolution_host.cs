// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

namespace Aksio.IngressMiddleware.integrationtests.route_and_then_claim_sourceidentifier.given;

public class multi_resolution_host : Specification
{
    protected HttpClient _ingressClient;
    protected Guid ExpectedRouteTenantId;
    protected string ExpectedRouteSourceIdentifier;
    protected Guid ExpectedEntraIdTenantId;
    protected string ExpectedEntraIdTenantSourceIdentifier;

    void Establish()
    {
        ExpectedRouteTenantId = Guid.NewGuid();
        ExpectedRouteSourceIdentifier = "1122";
        ExpectedEntraIdTenantSourceIdentifier = Guid.NewGuid().ToString();
        ExpectedEntraIdTenantId = Guid.NewGuid();
        
        var ingressConfig = new Config()
        {
            Tenants = new()
            {
                { Guid.NewGuid(), new() { SourceIdentifiers = new[] { "8844" } } },
                { ExpectedRouteTenantId, new() { SourceIdentifiers = new[] { ExpectedRouteSourceIdentifier } } },
                { ExpectedEntraIdTenantId, new() { SourceIdentifiers = new[] { ExpectedEntraIdTenantSourceIdentifier } } }
            },
            TenantResolutions = new[]
            {
                // First it should try regexp route matching.
                new TenantResolutionConfig()
                {
                    Strategy = TenantSourceIdentifierResolverType.Route,
                    Options = JsonSerializer.Deserialize<JsonObject>(
                        JsonSerializer.Serialize(
                            new RouteSourceIdentifierOptions() { RegularExpression = "^/(?<sourceIdentifier>[\\d]{4})/" }))
                },

                // And then it should fall back to principal claim matching.
                new TenantResolutionConfig()
                {
                    Strategy = TenantSourceIdentifierResolverType.Claim
                }
            },
            Authorization = new()
            {
                {
                    "audienceWithNoAuth", new() { NoAuthorizationRequired = true }
                }
            }
        };
            var ingressFactory = new IngressWebApplicationFactory { Config = ingressConfig
        };

        _ingressClient = ingressFactory.CreateClient();
    }

    protected void BuildAndSetPrincipalWithTenantClaim(
        HttpRequestMessage requestMessage,
        string claimedTenantId,
        string authAudience,
        params string[] roles)
    {
        var claims = new List<RawClaim>
        {
            new(ClaimsSourceIdentifier.TenantIdClaim, claimedTenantId),
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
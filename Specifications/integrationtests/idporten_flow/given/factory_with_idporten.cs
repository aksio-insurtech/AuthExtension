// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

namespace Aksio.IngressMiddleware.integrationtests.idporten_flow.given;

public class factory_with_idporten : Specification
{
    protected IngressWebApplicationFactory IngressFactory;
    protected HttpClient IngressClient;
    protected Config IngressConfig;
    protected Guid Tenant1Id;
    protected Guid Tenant2Id;

    void Establish()
    {
        Tenant1Id = Guid.NewGuid();
        Tenant2Id = Guid.NewGuid();

        IngressConfig = new()
        {
            IdPorten = new()
            {
                Issuer = "/idporten_issuer",
                AuthorizationEndpoint = "/idporten_authorization"
            },

            Tenants = new TenantsConfig()
            {
                { Tenant1Id, new TenantConfig() { Domain = "host1", OnBehalfOf = "obo1", SourceIdentifiers = new[] { "11" } } },
                { Tenant2Id, new TenantConfig() { Domain = "host2", OnBehalfOf = "obo2", SourceIdentifiers = new[] { "22" } } }
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
                                    { "host1", "sourceidentifier1" },
                                    { "host2", "sourceidentifier2" }
                                }
                            }))
                },
                new TenantResolutionConfig()
                {
                    Strategy = TenantSourceIdentifierResolverType.Claim
                }
            }
        };

        IngressFactory = new()
        {
            Config = IngressConfig
        };

        IngressClient = IngressFactory.CreateClient(new() { AllowAutoRedirect = false });
    }
}
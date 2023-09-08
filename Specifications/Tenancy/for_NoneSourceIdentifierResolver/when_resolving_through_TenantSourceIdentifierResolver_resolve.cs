// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy.for_NoneSourceIdentifierResolver;

public class when_resolving_through_TenantSourceIdentifierResolver_resolve : Specification
{
    SpecifiedSourceIdentifierResolver resolver;
    Config config;
    string expectedTenantId = "someTenantIdentifier";
    string resolvedTenant;

    void Establish()
    {
        var options = new SpecifiedSourceIdentifierResolverOptions() { TenantId = expectedTenantId };
        config = new()
        {
            TenantResolution = new()
            {
                Options = (JsonNode.Parse(JsonSerializer.Serialize(options, Globals.JsonSerializerOptions)) as JsonObject)!,
                Strategy = TenantSourceIdentifierResolverType.Specified
            }
        };
        resolver = new();
    }

    async Task Because() => resolvedTenant = await resolver.Resolve(config, null!);

    [Fact]
    void returned_correct_tenant() => resolvedTenant.ShouldEqual(expectedTenantId);
}
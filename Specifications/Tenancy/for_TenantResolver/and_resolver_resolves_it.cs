// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;

namespace Aksio.IngressMiddleware.Tenancy.for_TenantResolver.when_resolving;

public class and_resolver_resolves_it : given.a_tenant_resolver
{
    Guid tenant_id = Guid.Parse("c392e7be-5cb4-4d1b-a461-7077e197309c");
    TenantId result;

    void Establish()
    {
        config.Tenants[tenant_id] = new()
        {
            SourceIdentifiers = new[] { "3610" }
        };

        source_identifier_resolver.Setup(_ => _.Resolve(config, context.Request)).ReturnsAsync("3610");
    }

    async Task Because() => result = await resolver.Resolve(context.Request);

    [Fact]
    void should_resolve_to_the_tenant_id() => result.Value.ShouldEqual(tenant_id);
}
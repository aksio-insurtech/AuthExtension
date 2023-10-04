// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;

namespace Aksio.IngressMiddleware.Tenancy.for_TenantResolver;

public class and_resolver_resolves_it : given.a_tenant_resolver
{
    Guid _tenantId = Guid.Parse("c392e7be-5cb4-4d1b-a461-7077e197309c");
    TenantId _result;

    void Establish()
    {
        Config.Tenants[_tenantId] = new()
        {
            SourceIdentifiers = new[] { "3610" }
        };

        SourceIdentifierResolver.Setup(_ => _.Resolve(Config, Context.Request)).Returns("3610");
    }

    void Because() => _result = Resolver.Resolve(Context.Request);

    [Fact]
    void should_resolve_to_the_tenant_id() => _result.Value.ShouldEqual(_tenantId);
}
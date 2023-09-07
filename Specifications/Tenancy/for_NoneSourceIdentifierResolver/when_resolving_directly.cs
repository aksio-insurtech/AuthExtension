// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy.for_NoneSourceIdentifierResolver;

public class when_resolving_directly : Specification
{
    SpecifiedSourceIdentifierResolver resolver;
    SpecifiedSourceIdentifierResolverOptions options;
    string expectedTenantId = "someTenantIdentifier";
    string resolvedTenant;

    void Establish()
    {
        options = new() { TenantId = expectedTenantId };
        resolver = new();
    }

    async Task Because() => resolvedTenant = await resolver.Resolve(null!, options, null!);

    [Fact]
    void returned_correct_tenant() => resolvedTenant.ShouldEqual(expectedTenantId);
}
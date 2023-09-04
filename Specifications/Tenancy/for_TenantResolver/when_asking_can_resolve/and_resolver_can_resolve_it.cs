// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy.for_TenantResolver.when_asking_can_resolve;

public class and_resolver_can_resolve_it : given.a_tenant_resolver
{
    bool result;

    void Establish() => source_identifier_resolver.Setup(_ => _.CanResolve(config, context.Request)).ReturnsAsync(true);

    async Task Because() => result = await resolver.CanResolve(context.Request);

    [Fact] void should_be_able_to_resolve() => result.ShouldBeTrue();
}

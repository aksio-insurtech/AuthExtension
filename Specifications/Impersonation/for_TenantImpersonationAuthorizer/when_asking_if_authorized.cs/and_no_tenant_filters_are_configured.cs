// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Impersonation.for_TenantImpersonationAuthorizer.when_asking_if_authorized;

public class and_no_tenant_filters_are_configured : given.no_tenant_filters
{
    bool result;

    async Task Because() => result = await authorizer.IsAuthorized(http_context.Request, ClientPrincipal.Empty);

    [Fact]
    void should_be_authorized() => result.ShouldBeTrue();
}
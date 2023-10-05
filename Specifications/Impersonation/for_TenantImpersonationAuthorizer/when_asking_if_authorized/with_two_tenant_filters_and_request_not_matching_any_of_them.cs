// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;

namespace Aksio.IngressMiddleware.Impersonation.for_TenantImpersonationAuthorizer.when_asking_if_authorized;

public class with_two_tenant_filters_and_request_not_matching_any_of_them : given.two_tenant_filters
{
    bool _result;
    TenantId _unknownTenantId;

    void Establish()
    {
        _unknownTenantId = new TenantId(Guid.Parse("b3551c0e-9c68-40c5-9711-742042b5ed44"));
        TenantResolver.Setup(_ => _.TryResolve(HttpContext.Request, out _unknownTenantId)).Returns(true);
    }

    async Task Because() => _result = await Authorizer.IsAuthorized(HttpContext.Request, ClientPrincipal.Empty);

    [Fact]
    void should_not_be_authorized() => _result.ShouldBeFalse();
}
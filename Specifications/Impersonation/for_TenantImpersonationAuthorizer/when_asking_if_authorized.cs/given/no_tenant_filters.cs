// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Tenancy;

namespace Aksio.IngressMiddleware.Impersonation.for_TenantImpersonationAuthorizer.when_asking_if_authorized.given;

public class no_tenant_filters : Impersonation.given.a_http_context
{
    protected TenantImpersonationAuthorizer authorizer;
    protected Mock<ITenantResolver> tenant_resolver;

    void Establish()
    {
        var config = new Config();
        tenant_resolver = new();
        authorizer = new(config, tenant_resolver.Object);
    }
}

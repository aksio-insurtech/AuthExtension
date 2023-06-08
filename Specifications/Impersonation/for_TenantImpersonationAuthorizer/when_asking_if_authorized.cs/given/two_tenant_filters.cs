// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Tenancy;

namespace Aksio.IngressMiddleware.Impersonation.for_TenantImpersonationAuthorizer.when_asking_if_authorized.given;

public class two_tenant_filters : Impersonation.given.a_http_context
{
    protected static readonly string first_tenant = "09696cef-4885-4d1f-adcc-8fc680b8c599";
    protected static readonly string second_tenant = "b3b2ea06-0ece-4698-9f7d-4a11849f184b";

    protected TenantImpersonationAuthorizer authorizer;
    protected Mock<ITenantResolver> tenant_resolver;

    void Establish()
    {
        var config = new Config();
        config.Impersonation.Authorization.Tenants = new[]
        {
            first_tenant,
            second_tenant
        };
        tenant_resolver = new();
        authorizer = new(config, tenant_resolver.Object);
    }
}

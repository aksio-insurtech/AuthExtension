// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.given;
using Aksio.IngressMiddleware.Tenancy;

namespace Aksio.IngressMiddleware.Impersonation.for_TenantImpersonationAuthorizer.when_asking_if_authorized.given;

public class two_tenant_filters : a_http_context
{
    protected static readonly string FirstTenant = "09696cef-4885-4d1f-adcc-8fc680b8c599";
    protected static readonly string SecondTenant = "b3b2ea06-0ece-4698-9f7d-4a11849f184b";

    protected TenantImpersonationAuthorizer Authorizer;
    protected Mock<ITenantResolver> TenantResolver;

    void Establish()
    {
        var config = new Config();
        config.Impersonation.Authorization.Tenants = new[]
        {
            FirstTenant,
            SecondTenant
        };
        TenantResolver = new();
        Authorizer = new(config, TenantResolver.Object);
    }
}
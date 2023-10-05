// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.given;
using Aksio.IngressMiddleware.Tenancy;

namespace Aksio.IngressMiddleware.Impersonation.for_TenantImpersonationAuthorizer.when_asking_if_authorized.given;

public class no_tenant_filters : a_http_context
{
    protected TenantImpersonationAuthorizer Authorizer;
    protected Mock<ITenantResolver> TenantResolver;

    void Establish()
    {
        var config = new Config();
        TenantResolver = new();
        Authorizer = new(config, TenantResolver.Object);
    }
}
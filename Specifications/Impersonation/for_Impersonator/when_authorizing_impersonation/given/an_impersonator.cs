// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Identities;
using Aksio.IngressMiddleware.Tenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.Impersonation.for_Impersonator.when_authorizing_impersonation.given;

public class an_impersonator : Specification
{
    protected Impersonator Impersonator;
    protected Mock<IServiceProvider> ServiceProvider;
    protected Mock<IImpersonationAuthorizer> Authorizer;
    protected Mock<ITenantResolver> TenantResolver;
    protected Mock<IIdentityDetailsResolver> IdentityDetailsResolver;

    void Establish()
    {
        ServiceProvider = new();
        Authorizer = new();
        TenantResolver = new();
        IdentityDetailsResolver = new();
        ServiceProvider.Setup(_ => _.GetService(IsAny<Type>())).Returns(Authorizer.Object);

        Impersonator = new(
            ServiceProvider.Object,
            IdentityDetailsResolver.Object,
            TenantResolver.Object,
            Mock.Of<ILogger<Impersonator>>())
        {
            ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }
}
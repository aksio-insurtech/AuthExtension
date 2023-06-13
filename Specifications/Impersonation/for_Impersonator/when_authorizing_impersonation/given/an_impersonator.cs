// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.Impersonation.for_Impersonator.when_authorizing_impersonation.given;

public class an_impersonator : Specification
{
    protected Impersonator impersonator;
    protected Mock<IServiceProvider> service_provider;
    protected Mock<IImpersonationAuthorizer> authorizer;

    void Establish()
    {
        service_provider = new Mock<IServiceProvider>();
        authorizer = new();
        service_provider.Setup(_ => _.GetService(IsAny<Type>())).Returns(authorizer.Object);

        impersonator = new(service_provider.Object, Mock.Of<ILogger<Impersonator>>())
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }
}

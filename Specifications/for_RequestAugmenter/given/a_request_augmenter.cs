// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;
using Aksio.IngressMiddleware.BearerTokens;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Identities;
using Aksio.IngressMiddleware.Impersonation;
using Aksio.IngressMiddleware.MutualTLS;
using Aksio.IngressMiddleware.RoleAuthorization;
using Aksio.IngressMiddleware.Tenancy;
using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.for_RequestAugmenter.given;

public class a_request_augmenter : Specification
{
    protected static readonly TenantId TenantId = new(Guid.Parse("117a700a-f271-49af-a301-5d54cc2a8c9d"));
    protected Mock<IIdentityDetailsResolver> IdentityDetailsResolver;
    protected Mock<IImpersonationFlow> ImpersonationFlow;
    protected Mock<ITenantResolver> TenantResolver;
    protected Mock<IOAuthBearerTokens> BearerTokens;
    protected Mock<IMutualTLS> MutualTls;
    protected Mock<IRoleAuthorizer> EndtraidRoles;
    protected RequestAugmenter Augmenter;
    protected HttpRequest Request;
    protected HttpResponse Response;
    protected Config Config;

    void Establish()
    {
        IdentityDetailsResolver = new();
        ImpersonationFlow = new();
        TenantResolver = new();
        TenantResolver.Setup(_ => _.Resolve(IsAny<HttpRequest>())).Returns(TenantId);
        BearerTokens = new();
        MutualTls = new();
        EndtraidRoles = new();

        Augmenter = new(
            IdentityDetailsResolver.Object,
            ImpersonationFlow.Object,
            TenantResolver.Object,
            BearerTokens.Object,
            MutualTls.Object,
            EndtraidRoles.Object)
        {
            ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        Request = Augmenter.ControllerContext.HttpContext.Request;
        Response = Augmenter.ControllerContext.HttpContext.Response;
    }
}
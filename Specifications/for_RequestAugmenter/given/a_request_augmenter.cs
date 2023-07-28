// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Aksio.IngressMiddleware.BearerTokens;
using Aksio.IngressMiddleware.Identities;
using Aksio.IngressMiddleware.Impersonation;
using Aksio.IngressMiddleware.Tenancy;
using IngressMiddleware.MutualTLS;
using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.for_RequestAugmenter.given;

public class a_request_augmenter : Specification
{
    protected static readonly TenantId tenant_id = new(Guid.Parse("117a700a-f271-49af-a301-5d54cc2a8c9d"));
    protected Mock<IIdentityDetailsResolver> identity_details_resolver;
    protected Mock<IImpersonationFlow> impersonation_flow;
    protected Mock<ITenantResolver> tenant_resolver;
    protected Mock<IOAuthBearerTokens> bearer_tokens;
    protected Mock<IMutualTLS> mutual_tls;
    protected RequestAugmenter augmenter;
    protected HttpRequest request;
    protected HttpResponse response;

    void Establish()
    {
        identity_details_resolver = new();
        impersonation_flow = new();
        tenant_resolver = new();
        tenant_resolver.Setup(_ => _.Resolve(IsAny<HttpRequest>())).ReturnsAsync(tenant_id);
        bearer_tokens = new();
        mutual_tls = new();

        augmenter = new(
            identity_details_resolver.Object,
            impersonation_flow.Object,
            tenant_resolver.Object,
            bearer_tokens.Object,
            mutual_tls.Object)
        {
            ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        request = augmenter.ControllerContext.HttpContext.Request;
        response = augmenter.ControllerContext.HttpContext.Response;
    }
}
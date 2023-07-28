// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_RequestAugmenter;

public class when_handling_impersonated_route : given.a_request_augmenter
{
    IActionResult result;
    string cookie_value;

    void Establish()
    {
        identity_details_resolver.Setup(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id)).ReturnsAsync(true);
        bearer_tokens.Setup(_ => _.Handle(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id)).ReturnsAsync(new OkResult());
        cookie_value = Guid.NewGuid().ToString();

        impersonation_flow.Setup(_ => _.HandleImpersonatedPrincipal(IsAny<HttpRequest>(), IsAny<HttpResponse>())).Returns(true);
    }

    async Task Because() => result = await augmenter.Get();

    [Fact] void should_ask_impersonation_flow_to_handle_impersonated_principal() => impersonation_flow.Verify(_ => _.HandleImpersonatedPrincipal(IsAny<HttpRequest>(), IsAny<HttpResponse>()), Once);
    [Fact] void should_return_ok() => result.ShouldBeOfExactType<OkResult>();
    [Fact] void should_resolve_identity_details() => identity_details_resolver.Verify(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id), Once);
    [Fact] void should_handle_bearer_tokens() => bearer_tokens.Verify(_ => _.Handle(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id), Once);
}
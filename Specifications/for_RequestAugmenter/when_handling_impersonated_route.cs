// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_RequestAugmenter;

public class when_handling_impersonated_route : given.a_request_augmenter
{
    IActionResult _result;
    string _cookieValue;

    void Establish()
    {
        IdentityDetailsResolver.Setup(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), TenantId))
            .ReturnsAsync(true);
        BearerTokens.Setup(_ => _.IsEnabled()).Returns(true);
        BearerTokens.Setup(_ => _.Handle(IsAny<HttpRequest>(), IsAny<HttpResponse>(), TenantId)).ReturnsAsync(new OkResult());
        _cookieValue = Guid.NewGuid().ToString();

        ImpersonationFlow.Setup(_ => _.HandleImpersonatedPrincipal(IsAny<HttpRequest>(), IsAny<HttpResponse>())).Returns(true);
    }

    async Task Because() => _result = await Augmenter.Get();

    [Fact]
    void should_ask_impersonation_flow_to_handle_impersonated_principal() =>
        ImpersonationFlow.Verify(_ => _.HandleImpersonatedPrincipal(IsAny<HttpRequest>(), IsAny<HttpResponse>()), Once);

    [Fact]
    void should_return_ok() => _result.ShouldBeOfExactType<OkResult>();

    [Fact]
    void should_resolve_identity_details() =>
        IdentityDetailsResolver.Verify(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), TenantId), Once);

    [Fact]
    void should_handle_bearer_tokens() =>
        BearerTokens.Verify(_ => _.Handle(IsAny<HttpRequest>(), IsAny<HttpResponse>(), TenantId), Once);
}
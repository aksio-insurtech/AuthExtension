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

        var cookies = new Mock<IRequestCookieCollection>();
        request.Cookies = cookies.Object;
        cookies.SetupGet(_ => _[Cookies.Impersonation]).Returns(cookie_value);
        cookies.Setup(_ => _.ContainsKey(Cookies.Impersonation)).Returns(true);
    }

    async Task Because() => result = await augmenter.Get();

    [Fact] void should_set_request_principal_to_content_of_impersonation_cookie() => request.Headers[Headers.Principal].ToString().ShouldEqual(cookie_value);
    [Fact] void should_set_response_principal_to_content_of_impersonation_cookie() => response.Headers[Headers.Principal].ToString().ShouldEqual(cookie_value);
    [Fact] void should_return_ok() => result.ShouldBeOfExactType<OkResult>();
    [Fact] void should_resolve_identity_details() => identity_details_resolver.Verify(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id), Once);
    [Fact] void should_handle_bearer_tokens() => bearer_tokens.Verify(_ => _.Handle(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id), Once);
}

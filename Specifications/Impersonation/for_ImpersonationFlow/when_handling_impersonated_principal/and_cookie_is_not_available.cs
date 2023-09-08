// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.Impersonation.when_handling_impersonated_principal;

public class and_cookie_is_not_available : given.a_impersonation_flow
{
    DefaultHttpContext http_context;
    Mock<IRequestCookieCollection> cookies;
    bool result;

    void Establish()
    {
        http_context = new();
        cookies = new();
        http_context.Request.Cookies = cookies.Object;

        cookies.Setup(_ => _.ContainsKey(Cookies.Impersonation)).Returns(false);
    }

    void Because() => result = flow.HandleImpersonatedPrincipal(http_context.Request, http_context.Response);

    [Fact]
    void should_not_have_handled_it() => result.ShouldBeFalse();

    [Fact]
    void should_not_set_principal_on_request_header() =>
        http_context.Request.Headers.ContainsKey(Headers.Principal).ShouldBeFalse();

    [Fact]
    void should_not_set_principal_on_response_header() =>
        http_context.Response.Headers.ContainsKey(Headers.Principal).ShouldBeFalse();
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.Impersonation.for_ImpersonationFlow.when_handling_impersonated_principal;

public class and_cookie_is_not_available : given.a_impersonation_flow
{
    DefaultHttpContext _httpContext;
    Mock<IRequestCookieCollection> _cookies;
    bool _result;

    void Establish()
    {
        _httpContext = new();
        _cookies = new();
        _httpContext.Request.Cookies = _cookies.Object;

        _cookies.Setup(_ => _.ContainsKey(Cookies.Impersonation)).Returns(false);
    }

    void Because() => _result = Flow.HandleImpersonatedPrincipal(_httpContext.Request, _httpContext.Response);

    [Fact]
    void should_not_have_handled_it() => _result.ShouldBeFalse();

    [Fact]
    void should_not_set_principal_on_request_header() =>
        _httpContext.Request.Headers.ContainsKey(Headers.Principal).ShouldBeFalse();

    [Fact]
    void should_not_set_principal_on_response_header() =>
        _httpContext.Response.Headers.ContainsKey(Headers.Principal).ShouldBeFalse();
}
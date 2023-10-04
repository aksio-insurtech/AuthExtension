// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.Impersonation.for_ImpersonationFlow.when_handling_impersonated_principal;

public class and_cookie_is_available : given.a_impersonation_flow
{
    DefaultHttpContext _httpContext;
    Mock<IRequestCookieCollection> _cookies;
    bool _result;
    string _principal = "some-principal";

    void Establish()
    {
        _httpContext = new();
        _cookies = new();
        _httpContext.Request.Cookies = _cookies.Object;

        _cookies.Setup(_ => _[Cookies.Impersonation]).Returns(_principal);
        _cookies.Setup(_ => _.ContainsKey(Cookies.Impersonation)).Returns(true);
    }

    void Because() => _result = Flow.HandleImpersonatedPrincipal(_httpContext.Request, _httpContext.Response);

    [Fact]
    void should_have_handled_it() => _result.ShouldBeTrue();

    [Fact]
    void should_set_principal_on_request_header() => _httpContext.Request.Headers[Headers.Principal].ShouldEqual(_principal);

    [Fact]
    void should_set_principal_on_response_header() => _httpContext.Response.Headers[Headers.Principal].ShouldEqual(_principal);
}
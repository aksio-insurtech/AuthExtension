// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.Impersonation.for_ImpersonationFlow.when_asking_if_request_is_impersonation_route;

public class and_it_is_not : given.a_impersonation_flow
{
    bool _result;
    DefaultHttpContext _httpContext;

    void Establish()
    {
        _httpContext = new();
        _httpContext.Request.Headers[Headers.OriginalUri] = "/some/other/route";
    }

    void Because() => _result = Flow.IsImpersonateRoute(_httpContext.Request);

    [Fact]
    void should_not_be_the_impersonation_route() => _result.ShouldBeFalse();
}
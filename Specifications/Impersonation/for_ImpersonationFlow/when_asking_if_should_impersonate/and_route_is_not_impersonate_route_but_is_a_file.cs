// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.Impersonation.for_ImpersonationFlow.when_asking_if_should_impersonate;

public class and_route_is_not_impersonate_route_but_is_a_file : given.a_impersonation_flow
{
    DefaultHttpContext _httpContext;
    bool _result;

    void Establish()
    {
        _httpContext = new();
        _httpContext.Request.Headers[Headers.OriginalUri] = "/something/random/file.txt?query=string";
    }

    void Because() => _result = Flow.ShouldImpersonate(_httpContext.Request);

    [Fact]
    void should_not_impersonate() => _result.ShouldBeFalse();
}
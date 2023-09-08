// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.Impersonation.when_asking_if_request_is_impersonation_route;

public class and_it_is_not : given.a_impersonation_flow
{
    bool result;
    DefaultHttpContext http_context;

    void Establish()
    {
        http_context = new();
        http_context.Request.Headers[Headers.OriginalUri] = "/some/other/route";
    }

    void Because() => result = flow.IsImpersonateRoute(http_context.Request);

    [Fact]
    void should_not_be_the_impersonation_route() => result.ShouldBeFalse();
}
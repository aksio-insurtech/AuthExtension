// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.Impersonation.when_asking_if_should_impersonate;

public class and_route_is_impersonate_route : given.a_impersonation_flow
{
    DefaultHttpContext http_context;
    bool result;

    void Establish()
    {
        http_context = new();
        http_context.Request.Headers[Headers.OriginalUri] = WellKnownPaths.Impersonation;
    }

    void Because() => result = flow.ShouldImpersonate(http_context.Request);

    [Fact] void should_not_impersonate() => result.ShouldBeFalse();
}

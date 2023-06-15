// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_RequestAugmenter;

public class when_handling_regular_route_that_fails_identity_resolution : given.a_request_augmenter
{
    IActionResult result;

    void Establish()
    {
        identity_details_resolver.Setup(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id)).ReturnsAsync(false);
    }

    async Task Because() => result = await augmenter.Get();

    [Fact] void should_return_forbidden() => ((StatusCodeResult)result).StatusCode.ShouldEqual(StatusCodes.Status403Forbidden);
    [Fact] void should_never_handle_bearer_tokens() => bearer_tokens.Verify(_ => _.Handle(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id), Never);
}

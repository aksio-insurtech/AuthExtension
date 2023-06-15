// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_RequestAugmenter;

public class when_handling_regular_route_and_bearer_token_is_not_authorized : given.a_request_augmenter
{
    IActionResult result;
    IActionResult bearer_token_result;

    void Establish()
    {
        identity_details_resolver.Setup(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id)).ReturnsAsync(true);

        bearer_token_result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
        bearer_tokens.Setup(_ => _.Handle(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id)).ReturnsAsync(bearer_token_result);
    }

    async Task Because() => result = await augmenter.Get();

    [Fact] void should_return_result_from_bearer_tokens() => result.ShouldEqual(bearer_token_result);
    [Fact] void should_resolve_identity_details() => identity_details_resolver.Verify(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id), Once);
}

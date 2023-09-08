// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_RequestAugmenter;

public class when_handling_regular_route_and_all_is_valid : given.a_request_augmenter
{
    IActionResult result;

    void Establish()
    {
        identity_details_resolver.Setup(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id))
            .ReturnsAsync(true);
        bearer_tokens.Setup(_ => _.IsEnabled()).Returns(true);
        bearer_tokens.Setup(_ => _.Handle(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id)).ReturnsAsync(new OkResult());
    }

    async Task Because() => result = await augmenter.Get();

    [Fact]
    void should_return_ok() => result.ShouldBeOfExactType<OkResult>();

    [Fact]
    void should_resolve_identity_details() =>
        identity_details_resolver.Verify(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id), Once);

    [Fact]
    void should_handle_bearer_tokens() =>
        bearer_tokens.Verify(_ => _.Handle(IsAny<HttpRequest>(), IsAny<HttpResponse>(), tenant_id), Once);
}
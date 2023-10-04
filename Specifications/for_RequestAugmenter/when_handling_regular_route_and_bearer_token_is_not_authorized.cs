// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_RequestAugmenter;

public class when_handling_regular_route_and_bearer_token_is_not_authorized : given.a_request_augmenter
{
    IActionResult _result;
    IActionResult _bearerTokenResult;

    void Establish()
    {
        IdentityDetailsResolver.Setup(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), TenantId))
            .ReturnsAsync(true);
        BearerTokens.Setup(_ => _.IsEnabled()).Returns(true);
        _bearerTokenResult = new StatusCodeResult(StatusCodes.Status401Unauthorized);
        BearerTokens.Setup(_ => _.Handle(IsAny<HttpRequest>(), IsAny<HttpResponse>(), TenantId))
            .ReturnsAsync(_bearerTokenResult);
    }

    async Task Because() => _result = await Augmenter.Get();

    [Fact]
    void should_return_result_from_bearer_tokens() => _result.ShouldEqual(_bearerTokenResult);

    [Fact]
    void should_resolve_identity_details() =>
        IdentityDetailsResolver.Verify(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), TenantId), Once);
}
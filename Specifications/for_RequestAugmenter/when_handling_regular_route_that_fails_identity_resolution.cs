// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_RequestAugmenter;

public class when_handling_regular_route_that_fails_identity_resolution : given.a_request_augmenter
{
    IActionResult _result;

    void Establish() =>
        IdentityDetailsResolver.Setup(_ => _.Resolve(IsAny<HttpRequest>(), IsAny<HttpResponse>(), TenantId))
            .ReturnsAsync(false);

    async Task Because() => _result = await Augmenter.Get();

    [Fact]
    void should_return_forbidden() => ((StatusCodeResult)_result).StatusCode.ShouldEqual(StatusCodes.Status403Forbidden);

    [Fact]
    void should_never_handle_bearer_tokens() =>
        BearerTokens.Verify(_ => _.Handle(IsAny<HttpRequest>(), IsAny<HttpResponse>(), TenantId), Never);
}
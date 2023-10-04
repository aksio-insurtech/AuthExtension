// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_RequestAugmenter;

public class when_handling_non_impersonated_route_and_identity_provider_requires_impersonation : given.a_request_augmenter
{
    IActionResult _result;

    void Establish() => ImpersonationFlow.Setup(_ => _.ShouldImpersonate(Request)).Returns(true);

    async Task Because() => _result = await Augmenter.Get();

    [Fact]
    void should_set_redirect_header() =>
        Response.Headers[Headers.ImpersonationRedirect].ShouldEqual(WellKnownPaths.Impersonation);

    [Fact]
    void should_return_forbidden() => ((StatusCodeResult)_result).StatusCode.ShouldEqual(StatusCodes.Status401Unauthorized);

    [Fact]
    void should_not_resolve_identity_details() =>
        IdentityDetailsResolver.Verify(_ => _.Resolve(Request, Response, TenantId), Never);

    [Fact]
    void should_not_handle_bearer_tokens() => BearerTokens.Verify(_ => _.Handle(Request, Response, TenantId), Never);
}
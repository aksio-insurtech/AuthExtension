// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_RequestAugmenter;

public class when_handling_impersonation_route : given.a_request_augmenter
{
    IActionResult _result;

    void Establish() => ImpersonationFlow.Setup(_ => _.IsImpersonateRoute(Request)).Returns(true);

    async Task Because() => _result = await Augmenter.Get();

    [Fact]
    void should_return_ok() => _result.ShouldBeOfExactType<OkResult>();

    [Fact]
    void should_not_resolve_identity_details() =>
        IdentityDetailsResolver.Verify(_ => _.Resolve(Request, Response, TenantId), Never);

    [Fact]
    void should_not_handle_bearer_tokens() => BearerTokens.Verify(_ => _.Handle(Request, Response, TenantId), Never);
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_RequestAugmenter;

public class when_handling_impersonation_route : given.a_request_augmenter
{
    IActionResult result;

    void Establish() => impersonation_flow.Setup(_ => _.IsImpersonateRoute(request)).Returns(true);

    async Task Because() => result = await augmenter.Get();

    [Fact] void should_return_ok() => result.ShouldBeOfExactType<OkResult>();
    [Fact] void should_not_resolve_identity_details() => identity_details_resolver.Verify(_ => _.Resolve(request, response, tenant_id, false), Never);
    [Fact] void should_not_handle_bearer_tokens() => bearer_tokens.Verify(_ => _.Handle(request, response, tenant_id), Never);
}

// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Impersonation.for_ClaimImpersonationAuthorizer.when_asking_if_authorized;

public class and_no_claims_filters_are_configured : given.config_with_no_claims
{
    ClaimImpersonationAuthorizer _authorizer;
    bool _result;

    void Establish() => _authorizer = new(Config);

    async Task Because() => _result = await _authorizer.IsAuthorized(HttpContext.Request, ClientPrincipal.Empty);

    [Fact]
    void should_be_authorized() => _result.ShouldBeTrue();
}
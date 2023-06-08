// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation.for_ClaimImpersonationAuthorizer.when_asking_if_authorized;

public class and_no_claims_filters_are_configured : given.config_with_no_claims
{
    ClaimImpersonationAuthorizer authorizer;
    bool result;

    void Establish()
    {
        authorizer = new(config);
    }

    async Task Because() => result = await authorizer.IsAuthorized(http_context.Request, ClientPrincipal.Empty);

    [Fact] void should_be_authorized() => result.ShouldBeTrue();
}

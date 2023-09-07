// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware.Impersonation.for_ClaimImpersonationAuthorizer.when_asking_if_authorized;

public class and_claims_are_configured_and_user_has_one_of_them : given.config_with_two_claims
{
    ClaimImpersonationAuthorizer authorizer;
    bool result;
    ClientPrincipal principal;

    void Establish()
    {
        authorizer = new(config);

        principal = ClientPrincipal.Empty with
        {
            Claims = new[]
            {
                new Claim(second_claim_type, second_claim_value)
            }
        };
    }

    async Task Because() => result = await authorizer.IsAuthorized(http_context.Request, principal);

    [Fact]
    void should_not_be_authorized() => result.ShouldBeFalse();
}
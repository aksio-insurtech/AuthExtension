// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware.Impersonation.for_GroupsImpersonationAuthorizer.when_asking_if_authorized;

public class and_groups_are_configured_and_user_has_them_and_more : given.config_with_two_groups
{
    GroupsImpersonationAuthorizer authorizer;
    bool result;
    ClientPrincipal principal;

    void Establish()
    {
        authorizer = new(config);

        principal = ClientPrincipal.Empty with
        {
            Claims = new[]
            {
                new Claim("groups", second_group),
                new Claim("groups", first_group)
            }
        };
    }

    async Task Because() => result = await authorizer.IsAuthorized(http_context.Request, principal);

    [Fact] void should_be_authorized() => result.ShouldBeTrue();
}

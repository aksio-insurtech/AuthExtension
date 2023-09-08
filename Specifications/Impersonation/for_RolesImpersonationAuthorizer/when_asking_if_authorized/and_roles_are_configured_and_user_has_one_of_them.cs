// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Impersonation.for_RolesImpersonationAuthorizer.when_asking_if_authorized;

public class and_roles_are_configured_and_user_has_one_of_them : given.config_with_two_roles
{
    RolesImpersonationAuthorizer authorizer;
    bool result;
    ClientPrincipal principal;

    void Establish()
    {
        authorizer = new(config);

        principal = ClientPrincipal.Empty with
        {
            UserRoles = new[]
            {
                second_role
            }
        };
    }

    async Task Because() => result = await authorizer.IsAuthorized(http_context.Request, principal);

    [Fact]
    void should_not_be_authorized() => result.ShouldBeFalse();
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation.for_RolesImpersonationAuthorizer.when_asking_if_authorized.given;

public class config_with_two_roles : IngressMiddleware.Impersonation.given.a_http_context
{
    protected const string first_role = "first_role";
    protected const string second_role = "second_role";
    protected Config config;

    void Establish()
    {
        config = new Config();
        config.Impersonation.Authorization.Roles = new[]
        {
            first_role,
            second_role
        };
    }
}

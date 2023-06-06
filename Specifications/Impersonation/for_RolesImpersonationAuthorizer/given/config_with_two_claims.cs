// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware.Impersonation.for_RolesImpersonationAuthorizer.when_asking_if_authorized.given;

public class config_with_two_roles : Aksio.IngressMiddleware.Impersonation.given.a_http_context
{
    protected const string first_role = "first_claim";
    protected const string second_role = "second_claim";
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

public class config_with_no_claims : Aksio.IngressMiddleware.Impersonation.given.a_http_context
{
    protected Config config;

    void Establish()
    {
        config = new Config();
    }
}
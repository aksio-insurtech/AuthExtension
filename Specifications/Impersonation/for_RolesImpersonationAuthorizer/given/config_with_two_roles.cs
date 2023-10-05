// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.given;

namespace Aksio.IngressMiddleware.Impersonation.for_RolesImpersonationAuthorizer.given;

public class config_with_two_roles : a_http_context
{
    protected const string FirstRole = "first_role";
    protected const string SecondRole = "second_role";
    protected Config Config;

    void Establish()
    {
        Config = new();
        Config.Impersonation.Authorization.Roles = new[]
        {
            FirstRole,
            SecondRole
        };
    }
}
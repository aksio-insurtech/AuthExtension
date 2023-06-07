// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation.for_GroupsImpersonationAuthorizer.when_asking_if_authorized.given;

public class config_with_two_groups : IngressMiddleware.Impersonation.given.a_http_context
{
    protected const string first_group = "first_group";
    protected const string second_group = "second_group";
    protected Config config;

    void Establish()
    {
        config = new Config();
        config.Impersonation.Authorization.Groups = new[]
        {
            first_group,
            second_group
        };
    }
}

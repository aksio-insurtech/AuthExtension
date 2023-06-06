// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation.for_GroupsImpersonationAuthorizer.when_asking_if_authorized.given;

public class config_with_no_groups : IngressMiddleware.Impersonation.given.a_http_context
{
    protected Config config;

    void Establish()
    {
        config = new Config();
    }
}
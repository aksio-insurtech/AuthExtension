// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.Impersonation.for_IdentityProviderImpersonationAuthorizer.when_asking_if_authorized;

public class and_there_are_no_identity_providers_configured : given.a_http_context
{
    IdentityProviderImpersonationAuthorizer authorizer;
    bool result;

    void Establish()
    {
        var config = new Config();
        authorizer = new(config);
    }

    async Task Because() => result = await authorizer.IsAuthorized(http_context.Request, ClientPrincipal.Empty);

    [Fact] void should_not_be_authorized() => result.ShouldBeFalse();
}

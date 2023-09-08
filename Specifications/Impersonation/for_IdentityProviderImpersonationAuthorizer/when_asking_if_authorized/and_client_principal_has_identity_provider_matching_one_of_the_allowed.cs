// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation.for_IdentityProviderImpersonationAuthorizer.when_asking_if_authorized;

public class and_client_principal_has_identity_provider_matching_one_of_the_allowed : given.a_http_context
{
    IdentityProviderImpersonationAuthorizer authorizer;
    bool result;
    ClientPrincipal client_principal;

    void Establish()
    {
        var config = new Config();
        config.Impersonation.IdentityProviders = new[] { "aad", "twitter" };
        authorizer = new(config);

        client_principal = ClientPrincipal.Empty with { IdentityProvider = "twitter" };
    }

    async Task Because() => result = await authorizer.IsAuthorized(http_context.Request, client_principal);

    [Fact]
    void should_be_authorized() => result.ShouldBeTrue();
}
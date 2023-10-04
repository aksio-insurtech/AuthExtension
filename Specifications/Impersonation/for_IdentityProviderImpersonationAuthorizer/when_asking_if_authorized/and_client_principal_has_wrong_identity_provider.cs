// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation.for_IdentityProviderImpersonationAuthorizer.when_asking_if_authorized;

public class and_client_principal_has_wrong_identity_provider : given.a_http_context
{
    IdentityProviderImpersonationAuthorizer _authorizer;
    bool _result;
    ClientPrincipal _clientPrincipal;

    void Establish()
    {
        var config = new Config();
        config.Impersonation.IdentityProviders = new[] { "aad", "twitter" };
        _authorizer = new(config);

        _clientPrincipal = ClientPrincipal.Empty with { IdentityProvider = "something" };
    }

    async Task Because() => _result = await _authorizer.IsAuthorized(HttpContext.Request, _clientPrincipal);

    [Fact]
    void should_not_be_authorized() => _result.ShouldBeFalse();
}
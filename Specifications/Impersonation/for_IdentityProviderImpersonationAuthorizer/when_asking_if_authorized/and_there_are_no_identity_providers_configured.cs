// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation.for_IdentityProviderImpersonationAuthorizer.when_asking_if_authorized;

public class and_there_are_no_identity_providers_configured : given.a_http_context
{
    IdentityProviderImpersonationAuthorizer _authorizer;
    bool _result;

    void Establish()
    {
        var config = new Config();
        _authorizer = new(config);
    }

    async Task Because() => _result = await _authorizer.IsAuthorized(HttpContext.Request, ClientPrincipal.Empty);

    [Fact]
    void should_not_be_authorized() => _result.ShouldBeFalse();
}
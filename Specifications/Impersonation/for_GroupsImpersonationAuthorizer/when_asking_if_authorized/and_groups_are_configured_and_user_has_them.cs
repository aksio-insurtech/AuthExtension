// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware.Impersonation.for_GroupsImpersonationAuthorizer.when_asking_if_authorized;

public class and_groups_are_configured_and_user_has_them : given.config_with_two_groups
{
    GroupsImpersonationAuthorizer _authorizer;
    bool _result;
    ClientPrincipal _principal;

    void Establish()
    {
        _authorizer = new(Config);

        _principal = ClientPrincipal.Empty with
        {
            Claims = new[]
            {
                new Claim("groups", SecondGroup),
                new Claim("groups", FirstGroup)
            }
        };
    }

    async Task Because() => _result = await _authorizer.IsAuthorized(HttpContext.Request, _principal);

    [Fact]
    void should_be_authorized() => _result.ShouldBeTrue();
}
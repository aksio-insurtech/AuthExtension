// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation;

public class RolesImpersonationAuthorizer : IImpersonationAuthorizer
{
    readonly Config _config;

    public RolesImpersonationAuthorizer(
        Config config)
    {
        _config = config;
    }

    public Task<bool> IsAuthorized(HttpRequest request, ClientPrincipal principal)
    {
        if (!_config.Impersonation.Authorization.Roles.Any())
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(
            principal.UserRoles
                        .Any(_ =>
                            _config.Impersonation.Authorization.Roles.Any(role => role.Equals(_, StringComparison.InvariantCultureIgnoreCase))));
    }
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation;

/// <summary>
/// Represents an authorizer for impersonation based on roles.
/// </summary>
public class RolesImpersonationAuthorizer : IImpersonationAuthorizer
{
    readonly Config _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="RolesImpersonationAuthorizer"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    public RolesImpersonationAuthorizer(
        Config config)
    {
        _config = config;
    }

    /// <inheritdoc/>
    public Task<bool> IsAuthorized(HttpRequest request, ClientPrincipal principal)
    {
        if (!_config.Impersonation.Authorization.Roles.Any())
        {
            return Task.FromResult(true);
        }

        if (!principal.UserRoles.Any())
        {
            return Task.FromResult(false);
        }

        var authorized = _config.Impersonation.Authorization.Roles
            .All(_ => principal.UserRoles.Any(role => role.Equals(_, StringComparison.InvariantCultureIgnoreCase)));

        return Task.FromResult(authorized);
    }
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation;

/// <summary>
/// Represents an authorizer for impersonation based on claims.
/// </summary>
public class ClaimImpersonationAuthorizer : IImpersonationAuthorizer
{
    readonly Config _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimImpersonationAuthorizer"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    public ClaimImpersonationAuthorizer(Config config)
    {
        _config = config;
    }

    /// <inheritdoc/>
    public Task<bool> IsAuthorized(HttpRequest request, ClientPrincipal principal)
    {
        if (!_config.Impersonation.Authorization.Claims.Any())
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(principal.Claims
            .All(_ => _config
                .Impersonation.Authorization.Claims.Any(claim =>
                    claim.Type.Equals(_.Type, StringComparison.InvariantCultureIgnoreCase) &&
                    claim.Value.Equals(_.Value, StringComparison.InvariantCultureIgnoreCase))));
    }
}
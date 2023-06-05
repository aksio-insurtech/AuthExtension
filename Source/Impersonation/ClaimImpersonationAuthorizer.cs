// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation;

public class ClaimImpersonationAuthorizer : IImpersonationAuthorizer
{
    readonly Config _config;

    public ClaimImpersonationAuthorizer(Config config)
    {
        _config = config;
    }

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
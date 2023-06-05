// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation;

public class IdentityProviderImpersonationAuthorizer : IImpersonationAuthorizer
{
    readonly Config _config;

    public IdentityProviderImpersonationAuthorizer(Config config)
    {
        _config = config;
    }

    public Task<bool> IsAuthorized(HttpRequest request, ClientPrincipal principal) =>
        Task.FromResult(_config.Impersonation.IdentityProviders.Any(_ => _ == principal.IdentityProvider));
}
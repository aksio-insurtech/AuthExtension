// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation;

/// <summary>
/// Represents an authorizer for impersonation based on identity provider names.
/// </summary>
public class IdentityProviderImpersonationAuthorizer : IImpersonationAuthorizer
{
    readonly Config _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityProviderImpersonationAuthorizer"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    public IdentityProviderImpersonationAuthorizer(Config config)
    {
        _config = config;
    }

    /// <inheritdoc/>
    public Task<bool> IsAuthorized(HttpRequest request, ClientPrincipal principal) =>
        Task.FromResult(_config.Impersonation.IdentityProviders.Any(_ => _ == principal.IdentityProvider));
}
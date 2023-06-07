// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Helpers;

namespace Aksio.IngressMiddleware.Impersonation;

/// <summary>
/// Represents an implementation of <see cref="IImpersonationFlow"/>.
/// </summary>
public class ImpersonationFlow : IImpersonationFlow
{
    readonly Config _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImpersonationFlow"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    public ImpersonationFlow(Config config)
    {
        _config = config;
    }

    /// <inheritdoc/>
    public bool HandleImpersonatedPrincipal(HttpRequest request, HttpResponse response)
    {
        if (request.Cookies.ContainsKey(Cookies.Impersonation))
        {
            request.Headers[Headers.Principal] = request.Cookies[Cookies.Impersonation];
            response.Headers[Headers.Principal] = request.Cookies[Cookies.Impersonation];
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public bool IsImpersonateRoute(HttpRequest request) => request.IsImpersonateRoute();

    /// <inheritdoc/>
    public bool ShouldImpersonate(HttpRequest request)
    {
        if (!IsImpersonateRoute(request))
        {
            var principal = ClientPrincipal.FromBase64(request.Headers[Headers.PrincipalId], request.Headers[Headers.Principal]);
            if (_config.Impersonation.IdentityProviders.Any(_ => _.Equals(principal.IdentityProvider, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }
        }

        return false;
    }
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Collections;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Helpers;

namespace Aksio.IngressMiddleware.Impersonation;

/// <summary>
/// Represents an implementation of <see cref="IImpersonationFlow"/>.
/// </summary>
public class ImpersonationFlow : IImpersonationFlow
{
    readonly Config _config;
    readonly ILogger<ImpersonationFlow> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImpersonationFlow"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="logger">Logger for logging.</param>
    public ImpersonationFlow(
        Config config,
        ILogger<ImpersonationFlow> logger)
    {
        _config = config;
        _logger = logger;
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
        if (request.HasFileExtension())
        {
            return false;
        }

        _logger.LogInformation("Checking if request should be impersonated. IsImpersonateRoute: {IsImpersonateRoute}, HasPrincipal: {HasPrincipal}", IsImpersonateRoute(request), request.HasPrincipal());
        if (!IsImpersonateRoute(request) && request.HasPrincipal())
        {
            var principal = ClientPrincipal.FromBase64(request.Headers[Headers.PrincipalId], request.Headers[Headers.Principal]);

            _logger.LogInformation("Principal has identity provider: {IdentityProvider}", principal.IdentityProvider);

            _config.Impersonation.IdentityProviders.ForEach(_ => _logger.LogInformation("Configured identity provider: {IdentityProvider}", _));

            if (_config.Impersonation.IdentityProviders.Any(_ => _.Equals(principal.IdentityProvider, StringComparison.InvariantCultureIgnoreCase)))
            {
                _logger.LogInformation("Request should be impersonated");
                return true;
            }
        }

        _logger.LogInformation("Request should not be impersonated");

        return false;
    }
}
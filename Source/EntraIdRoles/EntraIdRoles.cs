// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Helpers;

namespace Aksio.IngressMiddleware.EntraIdRoles;

/// <summary>
/// Represents an implementation of <see cref="MutualTLS.IMutualTLS"/>.
/// </summary>
public class EntraIdRoles : IEntraIdRoles
{
    readonly Config _config;
    readonly ILogger<EntraIdRoles> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MutualTLS.MutualTLS"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="logger">Logger for logging.</param>
    public EntraIdRoles(Config config, ILogger<EntraIdRoles> logger)
    {
        _config = config;
        _logger = logger;
    }

    // /// <inheritdoc/>
    // public bool IsEnabled() => _config.MutualTLS.IsEnabled;

    /// <inheritdoc/>
    public IActionResult Handle(HttpRequest request)
    {
        // Get caller address, for logging purposes.
        var clientIp = request.Headers["X-Forwarded-For"].FirstOrDefault() ?? "(n/a)";

        if (!request.HasPrincipal())
        {
            _logger.ClientDidNotProvidePrincipal(clientIp);
            return new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }

        var principal = ClientPrincipal.FromBase64(request.Headers[Headers.PrincipalId], request.Headers[Headers.Principal]);
        var userRoles = principal.Claims.Where(c => c.Type == "roles").Select(c => c.Value).ToList();

        var matchedRoles = _config.EntraIdRoles.AcceptedRoles.Intersect(userRoles, StringComparer.OrdinalIgnoreCase).ToList();
        if (!matchedRoles.Any())
        {
            _logger.UserDidNotHaveAnyMatchingRoles(userRoles, clientIp);
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        _logger.UserLoggedInWithRoles(matchedRoles, clientIp);
        return new OkResult();
    }
}
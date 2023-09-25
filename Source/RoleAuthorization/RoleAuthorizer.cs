// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Helpers;

namespace Aksio.IngressMiddleware.RoleAuthorization;

/// <summary>
/// Represents an implementation of <see cref="IRoleAuthorizer"/>.
/// </summary>
public class RoleAuthorizer : IRoleAuthorizer
{
    readonly Config _config;
    readonly ILogger<RoleAuthorizer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleAuthorizer"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="logger">Logger for logging.</param>
    public RoleAuthorizer(Config config, ILogger<RoleAuthorizer> logger)
    {
        _config = config;
        _logger = logger;
    }

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

        var principalId = request.Headers[Headers.PrincipalId].FirstOrDefault() ?? string.Empty;

        if (_config.RoleAuthorization.NoRoleRequired)
        {
            _logger.AcceptingClientWithoutRole(principalId, clientIp);
            return new OkResult();
        }

        var principal = ClientPrincipal.FromBase64(principalId, request.Headers[Headers.Principal]);
        var userRoles = principal.Claims.Where(c => c.Type == "roles").Select(c => c.Value).ToList();

        var matchedRoles = _config.RoleAuthorization.AcceptedRoles.Intersect(userRoles, StringComparer.OrdinalIgnoreCase).ToList();
        if (!matchedRoles.Any())
        {
            _logger.UserDidNotHaveAnyMatchingRoles(principalId, userRoles, clientIp);
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        _logger.UserLoggedInWithRoles(principalId, matchedRoles, clientIp);
        return new OkResult();
    }
}
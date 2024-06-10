// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Helpers;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

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
    public IActionResult Handle(HttpRequest request, TenantId tenantId)
    {
        // Get caller address, for logging purposes.
        var clientIp = request.Headers["X-Forwarded-For"].FirstOrDefault() ?? "(n/a)";

        if (!request.HasPrincipal())
        {
            _logger.ClientDidNotProvidePrincipal(clientIp);
            return new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }

        var principalId = request.Headers[Headers.PrincipalId].FirstOrDefault() ?? string.Empty;
        var principal = ClientPrincipal.FromBase64(principalId, request.Headers[Headers.Principal]);

        var principalEmailOrId = principal.GetClientEmailOrIdentifier();
        var principalTenantId = principal.Claims.FirstOrDefault(c => c.Type == ClaimsSourceIdentifier.TenantIdClaim)?.Value ??
                                string.Empty;

        // Require the audience claim, this will represent the clientId which is used to authorize.
        // In EntraID this is the app registration id.
        if (!principal.Claims.Any(c => c.Type == "aud"))
        {
            _logger.ClientPrincipalDidNotHaveAudienceClaim(principalEmailOrId, principalTenantId, clientIp);
            return new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }

        var audience = principal.Claims.FirstOrDefault(c => c.Type == "aud")!.Value;
        if (!_config.Authorization.TryGetValue(audience, out var authorizationConfig))
        {
            _logger.ClientPrincipalAudienceIsNotConfigured(audience, principalEmailOrId, principalTenantId, clientIp);
            return new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }

        // Ensure that the entraid tenantId is allowed to log in with this middleware tenant, if a tenant is used.
        if (tenantId != TenantId.NotSet)
        {
            var tenantConfig = _config.Tenants[tenantId];
            if (!tenantConfig.EntraIdTenants.Contains(principalTenantId))
            {
                _logger.ClientPrincipalTenantIdDoesNotMatchMiddlewareTenantId(
                    tenantId,
                    principalEmailOrId,
                    principalTenantId,
                    clientIp);
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }

        if (authorizationConfig.NoAuthorizationRequired)
        {
            _logger.AcceptingClientWithoutRole(principalEmailOrId, principalEmailOrId, clientIp);
            return new OkResult();
        }

        var userRoles = principal.Claims.Where(c => c.Type == "roles").Select(c => c.Value).ToList();

        var matchedRoles = authorizationConfig.Roles.Intersect(userRoles, StringComparer.OrdinalIgnoreCase).ToList();
        if (!matchedRoles.Any())
        {
            _logger.UserDidNotHaveAnyMatchingRoles(principalEmailOrId, principalEmailOrId, userRoles, clientIp);
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        _logger.UserLoggedInWithRoles(principalEmailOrId, principalEmailOrId, matchedRoles, clientIp);
        return new OkResult();
    }
}
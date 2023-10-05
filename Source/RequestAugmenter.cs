// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;
using Aksio.IngressMiddleware.BearerTokens;
using Aksio.IngressMiddleware.Identities;
using Aksio.IngressMiddleware.Impersonation;
using Aksio.IngressMiddleware.MutualTLS;
using Aksio.IngressMiddleware.RoleAuthorization;
using Aksio.IngressMiddleware.Tenancy;

namespace Aksio.IngressMiddleware;

/// <summary>
/// Represents the root route for the ingress middleware.
/// </summary>
/// <remarks>
/// This is the default route used for authorizing and identity requests.
/// </remarks>
[Route("/")]
public class RequestAugmenter : Controller
{
    readonly IIdentityDetailsResolver _identityDetailsResolver;
    readonly IImpersonationFlow _impersonationFlow;
    readonly ITenantResolver _tenantResolver;
    readonly IOAuthBearerTokens _bearerTokens;
    readonly IMutualTLS _mutualTls;
    readonly IRoleAuthorizer _roleAuthorizer;
    readonly ILogger<RequestAugmenter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestAugmenter"/> class.
    /// </summary>
    /// <param name="identityDetailsResolver"><see cref="IIdentityDetailsResolver"/>.</param>
    /// <param name="impersonationFlow"><see cref="IImpersonationFlow"/> (for the impersonation process).</param>
    /// <param name="tenantResolver"><see cref="ITenantResolver"/>.</param>
    /// <param name="bearerTokens"><see cref="IOAuthBearerTokens"/>.</param>
    /// <param name="mutualTls"><see cref="IMutualTLS"/>.</param>
    /// <param name="roleAuthorizer"><see cref="IRoleAuthorizer"/>.</param>
    /// <param name="logger">The logger.</param>
    public RequestAugmenter(
        IIdentityDetailsResolver identityDetailsResolver,
        IImpersonationFlow impersonationFlow,
        ITenantResolver tenantResolver,
        IOAuthBearerTokens bearerTokens,
        IMutualTLS mutualTls,
        IRoleAuthorizer roleAuthorizer,
        ILogger<RequestAugmenter> logger)
    {
        _identityDetailsResolver = identityDetailsResolver;
        _impersonationFlow = impersonationFlow;
        _tenantResolver = tenantResolver;
        _bearerTokens = bearerTokens;
        _mutualTls = mutualTls;
        _roleAuthorizer = roleAuthorizer;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests to the root route.
    /// </summary>
    /// <returns><see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // First determine the tenant id, it will be populated for all strategies except "None" where it will be NotSet.
        // If null, requirements for setting a tenant is not present and the user is not authorized.
        if (!TryResolveTenantId(out var tenantId))
        {
            _logger.UnauthorizedBecauseNoTenantIdWasResolved();
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        // Handle mTLS, if enabled.
        if (_mutualTls.IsEnabled())
        {
            return _mutualTls.Handle(Request);
        }

        // Handle impersonation, if appropriate.
        if (!_impersonationFlow.HandleImpersonatedPrincipal(Request, Response) && _impersonationFlow.ShouldImpersonate(Request))
        {
            Response.Headers[Headers.ImpersonationRedirect] = WellKnownPaths.Impersonation;
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        if (_impersonationFlow.IsImpersonateRoute(Request))
        {
            return Ok();
        }

        // Use the identity details resolver, if configured.
        if (!await _identityDetailsResolver.Resolve(Request, Response, tenantId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        // Handle OAuth bearer tokens, if enabled.
        if (_bearerTokens.IsEnabled())
        {
            return await _bearerTokens.Handle(Request, Response, tenantId);
        }

        // Finally check the entra id requirement.
        return _roleAuthorizer.Handle(Request);
    }

    /// <summary>
    /// Attempts to resolve the tenant id, and set the Response.Header for tenantid based on the result.
    /// Will return false if it fails to determine one, and Response.Header for tenantid is removed.
    /// </summary>
    /// <param name="tenantId">The resolved tenant, TenantId.NotSet if resolver type is None.</param>
    /// <returns>True if successful, false if unable to resolve tenant.</returns>
    bool TryResolveTenantId(out TenantId tenantId)
    {
        if (!_tenantResolver.TryResolve(Request, out tenantId))
        {
            Response.Headers.Remove(Headers.TenantId);
            return false;
        }

        Response.Headers[Headers.TenantId] = tenantId.ToString();
        return true;
    }
}
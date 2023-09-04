// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.BearerTokens;
using Aksio.IngressMiddleware.Identities;
using Aksio.IngressMiddleware.Impersonation;
using Aksio.IngressMiddleware.Tenancy;
using IngressMiddleware.MutualTLS;

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

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestAugmenter"/> class.
    /// </summary>
    /// <param name="identityDetailsResolver"><see cref="IIdentityDetailsResolver"/> to use.</param>
    /// <param name="impersonationFlow"><see cref="IImpersonationFlow"/> to use for the impersonation process.</param>
    /// <param name="tenantResolver"><see cref="ITenantResolver"/> to use.</param>
    /// <param name="bearerTokens"><see cref="IOAuthBearerTokens"/> to use.</param>
    /// <param name="mutualTls"><see cref="IMutualTLS"/> to use.</param>
    public RequestAugmenter(
        IIdentityDetailsResolver identityDetailsResolver,
        IImpersonationFlow impersonationFlow,
        ITenantResolver tenantResolver,
        IOAuthBearerTokens bearerTokens,
        IMutualTLS mutualTls)
    {
        _identityDetailsResolver = identityDetailsResolver;
        _impersonationFlow = impersonationFlow;
        _tenantResolver = tenantResolver;
        _bearerTokens = bearerTokens;
        _mutualTls = mutualTls;
    }

    /// <summary>
    /// Handles GET requests to the root route.
    /// </summary>
    /// <returns><see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!await _tenantResolver.CanResolve(Request))
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        var tenantId = await _tenantResolver.Resolve(Request);
        Response.Headers[Headers.TenantId] = tenantId.ToString();

        if (_mutualTls.IsEnabled())
        {
            return _mutualTls.Handle(Request);
        }

        if (!_impersonationFlow.HandleImpersonatedPrincipal(Request, Response) &&
            _impersonationFlow.ShouldImpersonate(Request))
        {
            Response.Headers[Headers.ImpersonationRedirect] = WellKnownPaths.Impersonation;
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        if (_impersonationFlow.IsImpersonateRoute(Request))
        {
            return Ok();
        }

        if (!await _identityDetailsResolver.Resolve(Request, Response, tenantId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        return await _bearerTokens.Handle(Request, Response, tenantId);
    }
}
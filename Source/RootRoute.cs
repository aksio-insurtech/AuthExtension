// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.BearerTokens;
using Aksio.IngressMiddleware.Identities;
using Aksio.IngressMiddleware.Tenancy;

namespace Aksio.IngressMiddleware;

/// <summary>
/// Represents the root route for the ingress middleware.
/// </summary>
/// <remarks>
/// This is the default route used for authorizing and identity requests.
/// </remarks>
[Route("/")]
public class RootRoute : Controller
{
    readonly IIdentityDetailsResolver _identityDetailsResolver;
    readonly ITenantResolver _tenantResolver;
    readonly IOAuthBearerTokens _bearerTokens;

    /// <summary>
    /// Initializes a new instance of the <see cref="RootRoute"/> class.
    /// </summary>
    /// <param name="identityDetailsResolver"><see cref="IIdentityDetailsResolver"/> to use.</param>
    /// <param name="tenantResolver"><see cref="ITenantResolver"/> to use.</param>
    /// <param name="bearerTokens"><see cref="IOAuthBearerTokens"/> to use.</param>
    public RootRoute(
        IIdentityDetailsResolver identityDetailsResolver,
        ITenantResolver tenantResolver,
        IOAuthBearerTokens bearerTokens)
    {
        _identityDetailsResolver = identityDetailsResolver;
        _tenantResolver = tenantResolver;
        _bearerTokens = bearerTokens;
    }

    /// <summary>
    /// Handles GET requests to the root route.
    /// </summary>
    /// <returns><see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var tenantId = await _tenantResolver.Resolve(Request);
        Response.Headers[Headers.TenantId] = tenantId.ToString();

        // If we have an impersonation cookie, we'll set the principal header to the value of the cookie
        if( Request.Cookies.ContainsKey(Cookies.Impersonation))
        {
            Request.Headers[Headers.Principal] = Request.Cookies[Cookies.Impersonation];
            Response.Headers[Headers.Principal] = Request.Cookies[Cookies.Impersonation];
        }

        if (!await _identityDetailsResolver.Resolve(Request, Response, tenantId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        return await _bearerTokens.Handle(Request, Response, tenantId);
    }
}
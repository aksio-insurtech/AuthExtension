// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Helpers;
using Aksio.IngressMiddleware.Identities;
using Aksio.IngressMiddleware.Security;
using Aksio.IngressMiddleware.Tenancy;

namespace Aksio.IngressMiddleware.Impersonation;

/// <summary>
/// Represents the impersonation endpoints.
/// </summary>
[Route(WellKnownPaths.Impersonation)]
public class Impersonator : Controller
{
    static readonly IEnumerable<Type> _authorizers = new[]
    {
        typeof(TenantImpersonationAuthorizer),
        typeof(IdentityProviderImpersonationAuthorizer),
        typeof(ClaimImpersonationAuthorizer),
        typeof(RolesImpersonationAuthorizer),
        typeof(GroupsImpersonationAuthorizer)
    };
    readonly IServiceProvider _serviceProvider;
    readonly IIdentityDetailsResolver _identityDetailsResolver;
    readonly ITenantResolver _tenantResolver;
    readonly ILogger<Impersonator> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Impersonator"/> class.
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> to get instances from.</param>
    /// <param name="identityDetailsResolver"><see cref="IIdentityDetailsResolver"/> to use.</param>
    /// <param name="tenantResolver"><see cref="ITenantResolver"/> to use.</param>
    /// <param name="logger">Logger for logging.</param>
    public Impersonator(
        IServiceProvider serviceProvider,
        IIdentityDetailsResolver identityDetailsResolver,
        ITenantResolver tenantResolver,
        ILogger<Impersonator> logger)
    {
        _serviceProvider = serviceProvider;
        _identityDetailsResolver = identityDetailsResolver;
        _tenantResolver = tenantResolver;
        _logger = logger;
    }

    /// <summary>
    /// Impersonates a user based on HTTP POST with a form based on claims.
    /// </summary>
    /// <returns><see cref="IActionResult"/>.</returns>
    /// <remarks>
    /// The form should contain claims that will be used to impersonate the user.
    /// Claims are represented by `claim:type` and the value is the value of the claim.
    /// <br/>
    /// Example:
    /// <br/>
    /// claim:sub=1234567890.
    /// </remarks>
    [HttpGet("perform")]
    public async Task<IActionResult> Impersonate()
    {
        var principal = ClientPrincipal.FromBase64(Request.Headers[Headers.PrincipalId], Request.Headers[Headers.Principal]);
        _logger.PerformingImpersonation(principal.UserId, principal.UserDetails);
        var claims = Request.Query.ToClaims();
        var filtered = principal.Claims.Where(_ => !claims.Any(c => c.Type == _.Type));
        var newPrincipal = principal with
        {
            Claims = filtered.Concat(claims).ToArray()
        };

        var newPrincipalAsBase64 = newPrincipal.ToBase64();
        Response.Headers[Headers.Principal] = newPrincipalAsBase64;
        Response.Cookies.Append(Cookies.Impersonation, newPrincipalAsBase64, new CookieOptions { Expires = null! });

        var tenantId = await _tenantResolver.Resolve(Request);
        if (!await _identityDetailsResolver.Resolve(Request, Response, newPrincipalAsBase64, tenantId))
        {
            Response.Cookies.Delete(Cookies.Identity);
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        return Redirect("/");
    }

    /// <summary>
    /// Checks if the request is authorized for impersonation.
    /// </summary>
    /// <returns><see cref="IActionResult"/>.</returns>
    [HttpGet("auth")]
    public async Task<IActionResult> Authorize()
    {
        if (!Request.HasPrincipal())
        {
            _logger.NoPrincipal();
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var principal = ClientPrincipal.FromBase64(Request.Headers[Headers.PrincipalId], Request.Headers[Headers.Principal]);

        foreach (var authorizerType in _authorizers)
        {
            var authorizer = (_serviceProvider.GetRequiredService(authorizerType) as IImpersonationAuthorizer)!;
            if (!await authorizer.IsAuthorized(Request, principal))
            {
                _logger.ImpersonationNotAuthorized(principal.UserId, principal.UserDetails);
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        _logger.ImpersonationAuthorized(principal.UserId, principal.UserDetails);
        return Ok();
    }
}

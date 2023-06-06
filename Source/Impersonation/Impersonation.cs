// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware.Impersonation;

/// <summary>
/// Represents the impersonation endpoints.
/// </summary>
[Route("/.aksio/impersonate")]
public class Impersonation : Controller
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
    readonly ILogger<Impersonation> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Impersonation"/> class.
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> to get instances from.</param>
    /// <param name="logger">Logger for logging.</param>
    public Impersonation(
        IServiceProvider serviceProvider,
        ILogger<Impersonation> logger)
    {
        _serviceProvider = serviceProvider;
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
    /// claim:sub=1234567890
    /// </remarks>
    [HttpPost]
    public IActionResult Impersonate()
    {
        var claims = Request.Form.ToClaims();

        // Copy existing client principal
        // Replace claims with new claims
        // Add an impersonation cookie

        return Ok();
    }

    /// <summary>
    /// Checks if the request is authorized for impersonation.
    /// </summary>
    /// <returns><see cref="IActionResult"/>.</returns>
    [HttpGet("auth")]
    public async Task<IActionResult> Auth()
    {
        var principal = ClientPrincipal.FromBase64(Request.Headers[Headers.PrincipalId], Request.Headers[Headers.Principal]);

        foreach( var authorizerType in _authorizers)
        {
            var authorizer = (_serviceProvider.GetRequiredService(authorizerType) as IImpersonationAuthorizer)!;
            if (!await authorizer.IsAuthorized(Request, principal))
            {
                return Forbid();
            }
        }

        return Ok();
    }
}

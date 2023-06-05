// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware.Impersonation;

[Route("/.aksio/impersonate")]
public class Impersonation : Controller
{
    static IEnumerable<Type> _authorizers = new[]
    {
        typeof(TenantImpersonationAuthorizer),
        typeof(IdentityProviderImpersonationAuthorizer),
        typeof(ClaimImpersonationAuthorizer),
        typeof(RolesImpersonationAuthorizer),
        typeof(GroupsImpersonationAuthorizer)
    };
    readonly IServiceProvider _serviceProvider;
    readonly ILogger<Impersonation> _logger;

    public Impersonation(
        IServiceProvider serviceProvider,
        ILogger<Impersonation> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Impersonate()
    {
        var claims = Request.Form.ToClaims();

        // Copy existing client principal
        // Replace claims with new claims
        // Add an impersonation cookie

        return Ok();
    }


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

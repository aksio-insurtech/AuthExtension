// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Identities;
using Aksio.IngressMiddleware.Tenancy;

namespace Aksio.IngressMiddleware;

[Route("/")]
public class RootRoute : Controller
{
    readonly Config _config;
    readonly IIdentityDetailsResolver _identityDetailsResolver;
    readonly ITenantResolver _tenantResolver;

    public RootRoute(
        Config config,
        IIdentityDetailsResolver identityDetailsResolver,
        ITenantResolver tenantResolver)
    {
        _config = config;
        _identityDetailsResolver = identityDetailsResolver;
        _tenantResolver = tenantResolver;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var tenantId = await _tenantResolver.Resolve(Request, Response);

        // TODO: Impersonation. Look for impersonation cookie, if present, use that as the principal

        if (!await _identityDetailsResolver.Resolve(Request, Response, tenantId))
        {
            return Forbid();
        }

        //     await OAuthBearerTokens.HandleRequest(config, request, response, tenantId, httpClientFactory);

        return Ok();
    }
}
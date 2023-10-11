// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.integrationtests;

public class TestIdPortenEndpoint : Controller
{
    /// <summary>
    /// Emulates the idPorten authorization endpoint.
    /// In normal flow, this triggers the full auth flow and before it calls back to https://tenanthostname/.auth/login/members/callback?code=blabla.
    /// That url is then handled by the Azure Container App ingress, which in turn redirects to https://tenanthostname.
    /// So, this endpoint will fake the entire thing by just redirecting to the base-url in redirect_uri.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("/idporten_authorization")]
    public IActionResult IdPortenAuthorization()
    {
        var query = Request.Query.ToDictionary(_ => _.Key, _ => _.Value);
        var uri = new Uri(query["redirect_uri"].FirstOrDefault()!);

        // Redirect back to tenant's root
        return Redirect($"{uri.Scheme}://{uri.Host}");
    }
}
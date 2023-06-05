// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.IdPorten;

[Route("/id-porten")]
public class IdPorten : Controller
{
    readonly Config _config;

    public IdPorten(Config config)
    {
        _config = config;
    }

    [HttpGet("authorize")]
    public IActionResult Authorize()
    {
        var query = Request.Query.ToDictionary(_ => _.Key, _ => _.Value);
        var tenant = GetTenant();
        query["onbehalfof"] = tenant.Value.OnBehalfOf;

        var queryString = query.ToQueryString();
        var url = $"{_config.IdPorten.AuthorizationEndpoint}?{queryString}";

        return Redirect(url);
    }

    KeyValuePair<string, TenantConfig> GetTenant()
    {
        return Request.Query
            .Where(_ => _.Key == "redirect_uri")
            .Select(_ =>
            {
                var uri = new Uri(Uri.UnescapeDataString(_.Value));
                return _config.Tenants.First(_ => _.Value.Domain.Equals(uri.Host));
            })
            .Single();
    }
}

// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Helpers;

namespace Aksio.IngressMiddleware.IdPorten;

/// <summary>
/// Represents an endpoint for enabling IdPorten specific authorization.
/// </summary>
[Route("/id-porten")]
public class IdPorten : Controller
{
    readonly Config _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdPorten"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    public IdPorten(Config config)
    {
        _config = config;
    }

    /// <summary>
    /// Performs the authorization based on IdPorten and their 'onbehalfof'.
    /// </summary>
    /// <returns><see cref="IActionResult"/>.</returns>
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

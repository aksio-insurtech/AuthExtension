// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware;

public static class IdPorten
{
    public static async Task HandleAuthorize(Config config, HttpRequest request, HttpResponse response)
    {
        var query = request.Query.ToDictionary(_ => _.Key, _ => _.Value);
        var tenant = GetTenantFrom(config, request);
        query["onbehalfof"] = tenant.Value.OnBehalfOf;

        var queryString = query.ToQueryString();
        var url = $"{config.IdPorten.AuthorizationEndpoint}?{queryString}";
        response.Redirect(url);
        await Task.CompletedTask;
    }

    static KeyValuePair<string, TenantConfig> GetTenantFrom(Config config, HttpRequest request)
    {
        return request.Query
            .Where(_ => _.Key == "redirect_uri")
            .Select(_ =>
            {
                var uri = new Uri(Uri.UnescapeDataString(_.Value));
                return config.Tenants.First(_ => _.Value.Domain.Equals(uri.Host));
            })
            .Single();
    }
}

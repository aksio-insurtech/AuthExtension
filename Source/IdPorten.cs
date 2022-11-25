// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Nodes;

namespace Aksio.IngressMiddleware;

public static class IdPorten
{
    public static async Task HandleAuthorize(Config config, HttpRequest request, HttpResponse response)
    {
        var query = request.Query
                        // Id porten only supports the openid and profile scope, ignore anything else.
                        // .WithScope("openid+profile")
                        // .WithRedirectUri(config.IdPorten.Callback)
                        .ToDictionary(_ => _.Key, _ => _.Value);

        var tenant = GetTenantFrom(config, request);
        query["onbehalfof"] = tenant.Value.OnBehalfOf;
        //query["response_mode"] = "form_post";

        var queryString = query.ToQueryString();
        var url = $"{config.IdPorten.AuthorizationEndpoint}?{queryString}";
        response.Redirect(url);
        await Task.CompletedTask;
    }

    public static async Task HandleWellKnownConfiguration(Config config, HttpRequest request, HttpResponse response)
    {
        var client = new HttpClient();
        var url = $"{config.IdPorten.Issuer}/.well-known/openid-configuration";
        var result = await client.GetAsync(url);
        var json = await result.Content.ReadAsStringAsync();

        var document = (JsonNode.Parse(json) as JsonObject)!;
        document["authorization_endpoint"] = config.IdPorten.ProxyAuthorizationEndpoint;
        await response.WriteAsJsonAsync(document);
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

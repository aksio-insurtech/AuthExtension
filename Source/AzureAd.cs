// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Nodes;

namespace Aksio.IngressMiddleware;

public static class AzureAd
{
    public static async Task HandleAuthorize(Config config, HttpRequest request, HttpResponse response)
    {
        var query = request.Query.Select(_ => _.Key switch
            {
                // Change redirect URI to be configured callback - ourselves
                "redirect_uri" => new(_.Key, Uri.EscapeDataString(config.AzureAd.Callback)),

                _ => _
            }
        ).ToDictionary(_ => _.Key, _ => _.Value);
        var queryString = string.Join("&", query.Select(_ => string.Format($"{_.Key}={_.Value}")));
        var url = $"{config.AzureAd.TargetAuthorizationEndpoint}?{queryString}";
        response.Redirect(url);
        await Task.CompletedTask;
    }

    public static async Task HandleCallback(Config config, HttpRequest request, HttpResponse response)
    {
        if (request.Headers.ContainsKey("x-original-uri") &&
            request.Headers.ContainsKey("x-ai-original-host"))
        {
        }

        await Task.CompletedTask;
    }

    public static async Task HandleWellKnownConfiguration(Config config, HttpRequest request, HttpResponse response)
    {
        var client = new HttpClient();
        var url = $"{config.AzureAd.Issuer}/.well-known/openid-configuration";
        var result = await client.GetAsync(url);
        var json = await result.Content.ReadAsStringAsync();

        var document = (JsonNode.Parse(json) as JsonObject)!;
        document["authorization_endpoint"] = config.AzureAd.AuthorizationEndpoint;
        await response.WriteAsJsonAsync(document);
    }
}

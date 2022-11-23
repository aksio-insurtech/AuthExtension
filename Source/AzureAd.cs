// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Nodes;

namespace Aksio.IngressMiddleware;

public static class AzureAd
{
    public static async Task HandleWellKnownConfiguration(Config config, HttpRequest request, HttpResponse response)
    {
        var client = new HttpClient();
        var url = $"{config.AzureAd.Issuer}/.well-known/openid-configuration";
        var result = await client.GetAsync(url);
        var json = await result.Content.ReadAsStringAsync();

        var document = (JsonNode.Parse(json) as JsonObject)!;
        document["authorization_endpoint"] = config.AzureAd.AuthorizeEndpoint;
        await response.WriteAsJsonAsync(document);
    }
}

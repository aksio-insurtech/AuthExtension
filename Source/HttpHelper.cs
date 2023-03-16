// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace Aksio.IngressMiddleware;

public static class HttpHelper
{
    public static async Task<JsonDocument> PostAsync(string url, HttpContent? httpContent = null)
    {
        httpContent ??= new FormUrlEncodedContent(new Dictionary<string, string>());
        using var client = new HttpClient();

        Globals.Logger.LogInformation($"Posting to '{url}'");
        var response = await client.PostAsync(url, httpContent);

        Globals.Logger.LogInformation($"Status : {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }
}

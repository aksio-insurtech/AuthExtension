// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace Aksio.IngressMiddleware;

public static class HttpHelper
{
    public static async Task<JsonDocument> PostAsync(string url, HttpContent? httpContent = null)
    {
        httpContent ??= new FormUrlEncodedContent(new Dictionary<string, string>());
        var client = new HttpClient();
        var response = await client.PostAsync(url, httpContent);
        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }
}
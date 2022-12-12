// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware;

public static class Identity
{
    const string CookieName = ".aksio-identity";

    public static async Task HandleRequest(Config config, HttpRequest request, HttpResponse response, IHttpClientFactory httpClientFactory)
    {
        if (string.IsNullOrEmpty(config.IdentityDetailsUrl)) return;

        if (!request.Cookies.ContainsKey(CookieName)
            && request.Headers.ContainsKey(Headers.Principal)
            && request.Headers.ContainsKey(Headers.PrincipalId)
            && request.Headers.ContainsKey(Headers.PrincipalName))
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add(Headers.Principal, request.Headers[Headers.Principal].ToString());
                client.DefaultRequestHeaders.Add(Headers.PrincipalId, request.Headers[Headers.PrincipalId].ToString());
                client.DefaultRequestHeaders.Add(Headers.PrincipalName, request.Headers[Headers.PrincipalName].ToString());
                var responseMessage = await client.GetAsync(config.IdentityDetailsUrl);
                var identityDetails = await responseMessage.Content.ReadAsStringAsync();
                response.Cookies.Append(CookieName, identityDetails);
            }
            catch (Exception ex)
            {
                Globals.Logger.LogError(ex, "Error trying to resolve identity details");
            }
        }
        await Task.CompletedTask;
    }
}
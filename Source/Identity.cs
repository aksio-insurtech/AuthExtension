// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;
using System.Text;

namespace Aksio.IngressMiddleware;

public static class Identity
{
    const string CookieName = ".aksio-identity";

    public static async Task HandleRequest(Config config, HttpRequest request, HttpResponse response, IHttpClientFactory httpClientFactory)
    {
        if (string.IsNullOrEmpty(config.IdentityDetailsUrl))
        {
            Globals.Logger.LogInformation("Identity details url is not configured, skipping identity details resolution");
            return;
        }

        if (!request.Cookies.ContainsKey(CookieName)
            && request.Headers.ContainsKey(Headers.Principal)
            && request.Headers.ContainsKey(Headers.PrincipalId)
            && request.Headers.ContainsKey(Headers.PrincipalName))
        {
            try
            {
                Globals.Logger.LogInformation("Resolving identity details for {PrincipalId}", request.Headers[Headers.PrincipalId].ToString());

                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add(Headers.Principal, request.Headers[Headers.Principal].ToString());
                client.DefaultRequestHeaders.Add(Headers.PrincipalId, request.Headers[Headers.PrincipalId].ToString());
                client.DefaultRequestHeaders.Add(Headers.PrincipalName, request.Headers[Headers.PrincipalName].ToString());
                var responseMessage = await client.GetAsync(config.IdentityDetailsUrl);

                if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
                {
                    response.StatusCode = 403;
                }
                var identityDetails = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.StatusCode != HttpStatusCode.OK)
                {
                    Globals.Logger.LogError("Error trying to resolve identity details: {StatusCode} {ReasonPhrase}", responseMessage.StatusCode, responseMessage.ReasonPhrase);
                    return;
                }

                var identityDetailsAsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(identityDetails));
                response.Cookies.Append(CookieName, identityDetailsAsBase64, new CookieOptions { Expires = DateTimeOffset.Now.AddMinutes(5) });
            }
            catch (Exception ex)
            {
                Globals.Logger.LogError(ex, "Error trying to resolve identity details");
            }
        }
        else
        {
            Globals.Logger.LogInformation("No Identity resolved '{PrincipalId}', '{PrincipalName}', {Principal}",
                request.Headers[Headers.PrincipalId].ToString(),
                request.Headers[Headers.PrincipalName].ToString(),
                request.Headers[Headers.Principal].ToString());
        }
        await Task.CompletedTask;
    }
}
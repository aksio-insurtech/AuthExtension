// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json;

namespace Aksio.IngressMiddleware;

public static class AzureContainerAppAuth
{
    public const string AuthenticationCookie = ".zumo-authentication";

    public static async Task Login(OpenIDConnectConfig config, HttpRequest request, HttpResponse response, string idToken, string accessToken)
    {
        var loginUrl = $"https://{config.LoginUrl}/.auth/login/{config.AuthName}";

        var tokens = new
        {
            id_token = idToken,
            access_token = accessToken
        };
        var serializedTokens = JsonSerializer.Serialize(tokens);
        var loginContent = new StringContent(serializedTokens, Encoding.UTF8, "application/json");
        var loginResult = await HttpHelper.PostAsync(loginUrl, loginContent);

        var authenticationToken = loginResult.RootElement.GetProperty("authenticationToken").GetString()!;
        AddAuthenticationTokenAsCookie(request, response, authenticationToken);

    }

    public static void AddAuthenticationTokenAsCookie(HttpRequest request, HttpResponse response, string authenticationToken)
    {
        if (request.Cookies.ContainsKey(AuthenticationCookie))
        {
            response.Cookies.Delete(AuthenticationCookie);
        }
        response.Cookies.Append(AuthenticationCookie, authenticationToken);
    }

    public static async Task HandleZumoHeader(Config config, HttpRequest request, HttpResponse response)
    {
        if (request.Cookies.TryGetValue(AuthenticationCookie, out var authenticationCookie))
        {
            response.Headers.Add("X-ZUMO-AUTH", authenticationCookie);
        }

        await Task.CompletedTask;
    }

    public static void RedirectToOrigin(this HttpResponse response, HttpRequest request)
    {
        if (request.Headers.ContainsKey("x-original-uri") &&
            request.Headers.ContainsKey("x-ai-original-host"))
        {
            var url = $"https://{request.Headers["x-ai-original-host"]}{request.Headers["x-original-uri"]}";
            response.Redirect(url);
        }
    }
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace Aksio.IngressMiddleware;

public static class IdPorten
{
    const string AuthenticationCookie = ".idporten-authentication";

    public static async Task HandleAuthorize(Config config, HttpRequest request, HttpResponse response)
    {
        var query = request.Query.Select(_ => _.Key switch
            {
                // Id porten only supports the openid and profile scope, ignore anything else.
                "scope" => new(_.Key, "openid+profile"),

                // Change redirect URI to be configured callback - ourselves
                "redirect_uri" => new(_.Key, Uri.EscapeDataString(config.IdPorten.Callback)),

                _ => _
            }
        ).ToDictionary(_ => _.Key, _ => _.Value);

        var tenant = GetTenantFrom(config, request);
        query["onbehalfof"] = tenant.Value.OnBehalfOf;
        //query["response_mode"] = "form_post";

        var queryString = string.Join("&", query.Select(_ => string.Format($"{_.Key}={_.Value}")));

        var url = $"{config.IdPorten.AuthorizationEndpoint}?{queryString}";
        //QueryHelpers.AddQueryString(config.IdPorten.AuthorizationEndpoint, query);
        response.Redirect(url);
        await Task.CompletedTask;
    }

    public static async Task HandleCallback(Config config, HttpRequest request, HttpResponse response)
    {
        request.Query.TryGetValue("code", out var code);
        var url = QueryHelpers.AddQueryString(config.IdPorten.TokenEndpoint, new KeyValuePair<string, string?>[]
        {
            new("grant_type", "authorization_code"),
            new("code", code),
            new("client_id", config.IdPorten.ClientId),
            new("client_secret", config.IdPorten.ClientSecret)
        });

        var tokens = await PostAsync(url);
        var accessToken = tokens.RootElement.GetProperty("access_token").GetString()!;
        var idToken = tokens.RootElement.GetProperty("id_token").GetString()!;
        var accessTokenAsJWT = new JwtSecurityToken(accessToken);
        var onBehalfOf = accessTokenAsJWT.Claims.First(_ => _.Type == "client_onbehalfof").Value;

        var tenant = config.Tenants.First(_ => _.Value.OnBehalfOf == onBehalfOf);
        var loginUrl = $"https://{tenant.Value.Domain}/.auth/login/{config.IdPorten.AuthName}";

        var loginContent = new StringContent(tokens.RootElement.ToString(), Encoding.UTF8, "application/json");
        var loginResult = await PostAsync(loginUrl, loginContent);

        var authenticationToken = loginResult.RootElement.GetProperty("authenticationToken").GetString()!;
        if (request.Cookies.ContainsKey(AuthenticationCookie))
        {
            response.Cookies.Delete(AuthenticationCookie);
        }
        response.Cookies.Append(AuthenticationCookie, authenticationToken);

        var uri = $"https://{tenant.Value.Domain}";
        response.Redirect(uri);
        await Task.CompletedTask;
    }

    public static async Task HandleZumoHeader(Config config, HttpRequest request, HttpResponse response)
    {
        if (request.Cookies.TryGetValue(AuthenticationCookie, out var authenticationCookie))
        {
            response.Headers.Add("X-ZUMO-AUTH", authenticationCookie);
        }

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

    static async Task<JsonDocument> PostAsync(string url, HttpContent? httpContent = null)
    {
        httpContent ??= new FormUrlEncodedContent(new Dictionary<string, string>());
        var client = new HttpClient();
        var response = await client.PostAsync(url, httpContent);
        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }
}

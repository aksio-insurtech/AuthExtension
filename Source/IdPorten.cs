// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IdentityModel.Tokens.Jwt;

namespace Aksio.IngressMiddleware;

public static class IdPorten
{
    public static async Task HandleAuthorize(Config config, HttpRequest request, HttpResponse response)
    {
        var query = request.Query
                        // Id porten only supports the openid and profile scope, ignore anything else.
                        .WithScope("openid+profile")
                        .WithRedirectUri(config.IdPorten.Callback)
                        .ToDictionary(_ => _.Key, _ => _.Value);

        var tenant = GetTenantFrom(config, request);
        query["onbehalfof"] = tenant.Value.OnBehalfOf;
        //query["response_mode"] = "form_post";

        var queryString = query.ToQueryString();
        var url = $"{config.IdPorten.AuthorizationEndpoint}?{queryString}";
        response.Redirect(url);
        await Task.CompletedTask;
    }

    public static async Task HandleCallback(Config config, HttpRequest request, HttpResponse response)
    {
        request.Query.TryGetValue("code", out var code);
        var tokens = await OpenIDConnect.ExchangeCodeForAccessToken(config.IdPorten, code);
        var accessTokenAsJWT = new JwtSecurityToken(tokens.AccessToken);
        var onBehalfOf = accessTokenAsJWT.Claims.First(_ => _.Type == "client_onbehalfof").Value;

        await AzureContainerAppAuth.Login(config.IdPorten, request, response, tokens);

        var tenant = config.Tenants.First(_ => _.Value.OnBehalfOf == onBehalfOf);
        var uri = $"https://{tenant.Value.Domain}";
        response.Redirect(uri);
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

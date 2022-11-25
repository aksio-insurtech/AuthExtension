// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Web;

namespace Aksio.IngressMiddleware;

public static class Origin
{
    const string OriginCookie = ".origin";

    public static void SetOriginRedirectUri(Config config, HttpResponse response, string origin)
    {
        Globals.Logger.LogInformation("Origin header found - adding origin cookie");
        response.Cookies.Append(OriginCookie, origin, new()
        {
            Domain = config.CookieDomain,
            Expires = DateTimeOffset.UtcNow.AddHours(1),
            MaxAge = TimeSpan.FromHours(1),
            SameSite = SameSiteMode.None,
            HttpOnly = true,
            Secure = true,
            IsEssential = true
        });
    }

    public static void RedirectToOrigin(this HttpResponse response, HttpRequest request)
    {
        if (request.Cookies.ContainsKey(OriginCookie))
        {
            var redirectUri = HttpUtility.UrlDecode(request.Cookies[OriginCookie]);
            Globals.Logger.LogInformation($"Redirecting to {redirectUri} with queryString {request.QueryString}");
            response.Redirect($"{redirectUri}{request.QueryString}");
        }
    }
}
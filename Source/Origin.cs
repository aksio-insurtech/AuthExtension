// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware;

public static class Origin
{
    const string OriginHeader = "x-ai-origin-uri";
    const string OriginCookie = ".origin";

    public static void Handle(Config config, HttpRequest request, HttpResponse response)
    {
        if (request.Headers.ContainsKey(OriginHeader))
        {
            if (request.Cookies.ContainsKey(OriginCookie))
            {
                return;
            }

            response.Cookies.Append(OriginCookie, request.Headers[OriginHeader], new() { Domain = config.CookieDomain });
        }
    }

    public static void RedirectToOrigin(this HttpResponse response, HttpRequest request)
    {
        if (request.Cookies.ContainsKey(OriginCookie))
        {
            response.Redirect(request.Cookies[OriginCookie]!);
        }
    }
}
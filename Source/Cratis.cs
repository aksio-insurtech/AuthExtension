// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.NginxMiddleware;

public static class Cratis
{
    public static async Task HandleRequest(HttpRequest request, HttpResponse response)
    {
        response.Headers["Something"] = "x-qasdasd";
        await Task.CompletedTask;
    }
}
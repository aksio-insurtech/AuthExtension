// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.NginxMiddleware;

public static class Cratis
{
    public static async Task HandleRequest(Config config, HttpRequest request, HttpResponse response)
    {
        var tenant = config.Tenants.FirstOrDefault(_ => _.Value.Domain.Equals(request.Host.Host));
        response.Headers["Tenant-ID"] = tenant.Key;
        await Task.CompletedTask;
    }
}

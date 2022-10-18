// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace Aksio.NginxMiddleware;

public static class Cratis
{
    const string TenantsConfigFile = "tenants.json";

    static readonly IDictionary<Guid, IEnumerable<string>> _tenantConfig;

    static Cratis()
    {
        if (File.Exists(TenantsConfigFile))
        {
            var json = File.ReadAllText(TenantsConfigFile);
            _tenantConfig = JsonSerializer.Deserialize<IDictionary<Guid, IEnumerable<string>>>(json)!;
        }
        else
        {
            _tenantConfig = new Dictionary<Guid, IEnumerable<string>>();
        }
    }

    public static async Task HandleRequest(HttpRequest request, HttpResponse response)
    {
        var tenant = _tenantConfig.FirstOrDefault(_ => _.Value.Contains(request.Host.Host));
        var tenantId = tenant.Key;
        response.Headers["Tenant-ID"] = tenantId.ToString();
        await Task.CompletedTask;
    }
}

// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json.Nodes;

namespace Aksio.IngressMiddleware;

public static class Cratis
{
    const string PrincipalHeader = "x-ms-client-principal";
    const string TenantIdHeader = "Tenant-ID";

    public static async Task HandleRequest(Config config, HttpRequest request, HttpResponse response)
    {
        var tenantId = string.Empty;
        if (request.Headers.ContainsKey(PrincipalHeader))
        {
            var token = Convert.FromBase64String(request.Headers[PrincipalHeader]);
            var decodedToken = Encoding.Default.GetString(token);

            var node = JsonNode.Parse(token) as JsonObject;
            if (node is not null && node.TryGetPropertyValue("claims", out var claims) && claims is JsonArray claimsAsArray)
            {
                var tenantObject = claimsAsArray.Cast<JsonObject>().FirstOrDefault(_ => _.TryGetPropertyValue("typ", out var type) && type!.ToString() == "http://schemas.microsoft.com/identity/claims/tenantid");
                if (tenantObject is not null && tenantObject.TryGetPropertyValue("val", out var tenantValue) && tenantValue is not null)
                {
                    var tenantValueString = tenantValue.ToString();
                    var tenant = config.Tenants.FirstOrDefault(_ => _.Value.TenantIdClaims.Any(t => t == tenantValueString));
                    tenantId = tenant.Key;
                    Globals.Logger.LogInformation($"Setting tenant id to '{tenant.Key}' based on configured TID claim ({tenantValueString})");
                }
            }
        }

        if (string.IsNullOrEmpty(tenantId))
        {
            var tenant = config.Tenants.FirstOrDefault(_ => _.Value.Domain.Equals(request.Host.Host));
            tenantId = tenant.Key;
            Globals.Logger.LogInformation($"Setting tenant id to '{tenant.Key}' based on configured host ({request.Host.Host})");
        }
        response.Headers[TenantIdHeader] = tenantId;
        await Task.CompletedTask;
    }
}

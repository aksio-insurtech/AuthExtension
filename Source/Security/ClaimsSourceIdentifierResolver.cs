// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json.Nodes;

namespace Aksio.IngressMiddleware.Security;

public class ClaimsSourceIdentifierResolver : TenantSourceIdentifierResolver, ITenantSourceIdentifierResolver<ClaimsSourceIdentifierResolverOptions>
{
    public Task<string> Resolve(Config config, ClaimsSourceIdentifierResolverOptions options, HttpRequest request)
    {
        var sourceIdentifier = string.Empty;

        if (request.Headers.ContainsKey(Headers.Principal))
        {
            var token = Convert.FromBase64String(request.Headers[Headers.Principal]);
            var decodedToken = Encoding.Default.GetString(token);

            var node = JsonNode.Parse(token) as JsonObject;
            if (node is not null && node.TryGetPropertyValue("claims", out var claims) && claims is JsonArray claimsAsArray)
            {
                var tenantObject = claimsAsArray.Cast<JsonObject>().FirstOrDefault(_ => _.TryGetPropertyValue("typ", out var type) && type!.ToString() == "http://schemas.microsoft.com/identity/claims/tenantid");
                if (tenantObject is not null && tenantObject.TryGetPropertyValue("val", out var tenantValue) && tenantValue is not null)
                {
                    return Task.FromResult(tenantValue.ToString());
                }
            }
        }

        return Task.FromResult(sourceIdentifier);
    }
}

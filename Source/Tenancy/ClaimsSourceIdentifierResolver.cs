using System.Threading.Tasks;
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Represents a source identifier resolver for claims.
/// </summary>
public class ClaimsSourceIdentifierResolver : TenantSourceIdentifierResolver, ITenantSourceIdentifierResolver<ClaimsSourceIdentifierResolverOptions>
{
    const string TenantIdClaim = "http://schemas.microsoft.com/identity/claims/tenantid";

    /// <inheritdoc/>
    public Task<bool> CanResolve(Config config, ClaimsSourceIdentifierResolverOptions options, HttpRequest request) => Task.FromResult(TryGetTenantId(request, out _));

    /// <inheritdoc/>
    public Task<string> Resolve(Config config, ClaimsSourceIdentifierResolverOptions options, HttpRequest request)
    {
        if (TryGetTenantId(request, out var tenant))
        {
            return Task.FromResult(tenant);
        }

        return Task.FromResult(string.Empty);
    }

    bool TryGetTenantId(HttpRequest request, out string tenant)
    {
        if (request.Headers.ContainsKey(Headers.Principal))
        {
            var token = Convert.FromBase64String(request.Headers[Headers.Principal]);
            var decodedToken = Encoding.Default.GetString(token);

            var node = JsonNode.Parse(token) as JsonObject;
            if (node is not null && node.TryGetPropertyValue("claims", out var claims) && claims is JsonArray claimsAsArray)
            {
                var tenantObject = claimsAsArray.Cast<JsonObject>().FirstOrDefault(_ => _.TryGetPropertyValue("typ", out var type) && type!.ToString() == TenantIdClaim);
                if (tenantObject is not null && tenantObject.TryGetPropertyValue("val", out var tenantValue) && tenantValue is not null)
                {
                    tenant = tenantValue.ToString();
                    return true;
                }
            }
        }

        tenant = string.Empty;
        return false;
    }
}

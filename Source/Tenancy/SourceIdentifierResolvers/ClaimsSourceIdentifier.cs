// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

/// <summary>
/// Represents a source identifier resolver for claims.
/// </summary>
public class ClaimsSourceIdentifier : ISourceIdentifier
{
    /// <summary>
    /// The tenant id claim.
    /// </summary>
    public const string TenantIdClaim = "http://schemas.microsoft.com/identity/claims/tenantid";

    /// <inheritdoc/>
    public TenantSourceIdentifierResolverType ResolverType => TenantSourceIdentifierResolverType.Claim;

    readonly ILogger<ClaimsSourceIdentifier> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimsSourceIdentifier"/> class.
    /// </summary>
    /// <param name="logger">Logger for logging.</param>
    public ClaimsSourceIdentifier(ILogger<ClaimsSourceIdentifier> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public string? Resolve(JsonObject options, HttpRequest request)
    {
        if (request.Headers.TryGetValue(Headers.Principal, out var header))
        {
            var token = Convert.FromBase64String(header);
            if (JsonNode.Parse(token) is not JsonObject node)
            {
                return null;
            }

            if (node.TryGetPropertyValue("claims", out var claims) && claims is JsonArray claimsAsArray)
            {
                var tenantObject = claimsAsArray.Cast<JsonObject>()
                    .FirstOrDefault(_ => _.TryGetPropertyValue("typ", out var type) && type!.ToString() == TenantIdClaim);
                if (tenantObject is not null && tenantObject.TryGetPropertyValue("val", out var tenantValue) &&
                    tenantValue is not null)
                {
                    _logger.SettingSourceIdentifierBasedOnTenantClaim(tenantValue.ToString());

                    return tenantValue.ToString();
                }
            }
        }

        _logger.TenantClaimNotMatched();
        return null;
    }
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

public class TenantResolver
{
    readonly static Dictionary<TenantSourceIdentifierResolverType, ITenantSourceIdentifierResolver> _resolvers = new()
    {
        { TenantSourceIdentifierResolverType.Claim, new ClaimsSourceIdentifierResolver() },
        { TenantSourceIdentifierResolverType.Route, new RouteSourceIdentifierResolver() }
    };
    readonly Config _config;

    public TenantResolver(Config config)
    {
        _config = config;
    }

    public async Task<TenantId> Resolve(HttpRequest request, HttpResponse response)
    {
        var tenantId = string.Empty;
        var sourceIdentifier = string.Empty;
        if (_resolvers.ContainsKey(_config.TenantResolution.Strategy))
        {
            sourceIdentifier = await _resolvers[_config.TenantResolution.Strategy].Resolve(config, request);
        }

        if (!string.IsNullOrEmpty(sourceIdentifier))
        {
            var tenant = _config.Tenants.FirstOrDefault(_ => _.Value.SourceIdentifiers.Any(t => t == sourceIdentifier));
            tenantId = tenant.Key;
            if (!string.IsNullOrEmpty(tenantId))
            {
                Globals.Logger.LogInformation($"Setting tenant id to '{tenant.Key}' based on source identifier ({sourceIdentifier}) resolved using {config.TenantResolution.Strategy}");
            }
        }

        if (string.IsNullOrEmpty(tenantId))
        {
            var tenant = _config.Tenants.FirstOrDefault(_ => _.Value.Domain.Equals(request.Host.Host));
            tenantId = tenant.Key;
            if (!string.IsNullOrEmpty(tenantId))
            {
                Globals.Logger.LogInformation($"Setting tenant id to '{tenant.Key}' based on configured host ({request.Host.Host})");
            }
        }

        if (string.IsNullOrEmpty(tenantId))
        {
            Globals.Logger.LogInformation($"TenantId is not resolved, setting to empty.");
        }

        response.Headers[Headers.TenantId] = tenantId ?? string.Empty;
        return string.IsNullOrEmpty(tenantId) ? TenantId.NotSet : new TenantId(Guid.Parse(tenantId));
    }
}

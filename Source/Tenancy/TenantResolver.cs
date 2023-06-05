// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

public class TenantResolver : ITenantResolver
{
    readonly Config _config;
    readonly ITenantSourceIdentifierResolver _resolver;
    readonly ILogger<TenantResolver> _logger;

    public TenantResolver(Config config, ITenantSourceIdentifierResolver resolver, ILogger<TenantResolver> logger)
    {
        _config = config;
        _resolver = resolver;
        _logger = logger;
    }

    public async Task<TenantId> Resolve(HttpRequest request)
    {
        var tenantId = string.Empty;
        var sourceIdentifier = await _resolver.Resolve(_config, request);

        if (!string.IsNullOrEmpty(sourceIdentifier))
        {
            var tenant = _config.Tenants.FirstOrDefault(_ => _.Value.SourceIdentifiers.Any(t => t == sourceIdentifier));
            tenantId = tenant.Key;
            if (!string.IsNullOrEmpty(tenantId))
            {
                _logger.LogInformation($"Setting tenant id to '{tenant.Key}' based on source identifier ({sourceIdentifier}) resolved using {_config.TenantResolution.Strategy}");
            }
        }

        if (string.IsNullOrEmpty(tenantId))
        {
            var tenant = _config.Tenants.FirstOrDefault(_ => _.Value.Domain.Equals(request.Host.Host));
            tenantId = tenant.Key;
            if (!string.IsNullOrEmpty(tenantId))
            {
                _logger.LogInformation($"Setting tenant id to '{tenant.Key}' based on configured host ({request.Host.Host})");
            }
        }

        if (string.IsNullOrEmpty(tenantId))
        {
            _logger.LogInformation("TenantId is not resolved, setting to empty.");
        }

        return string.IsNullOrEmpty(tenantId) ? TenantId.NotSet : new TenantId(Guid.Parse(tenantId));
    }
}

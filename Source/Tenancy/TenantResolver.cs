// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="ITenantResolver"/>.
/// </summary>
public class TenantResolver : ITenantResolver
{
    readonly Config _config;
    readonly ITenantSourceIdentifierResolver _resolver;
    readonly ILogger<TenantResolver> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TenantResolver"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="resolver">The <see cref="ITenantSourceIdentifierResolver"/> to use.</param>
    /// <param name="logger">Logger for logging.</param>
    public TenantResolver(Config config, ITenantSourceIdentifierResolver resolver, ILogger<TenantResolver> logger)
    {
        _config = config;
        _resolver = resolver;
        _logger = logger;
    }

    /// <inheritdoc/>
    public Task<bool> CanResolve(HttpRequest request) => _resolver.CanResolve(_config, request);

    /// <inheritdoc/>
    public async Task<TenantId> Resolve(HttpRequest request)
    {
        var sourceIdentifier = await _resolver.Resolve(_config, request);

        if (!string.IsNullOrEmpty(sourceIdentifier))
        {
            _logger.AttemptingToResolveUsingSourceIdentifier();

            var tenantId = _config.Tenants.FirstOrDefault(_ => _.Value.SourceIdentifiers.Any(t => t == sourceIdentifier)).Key;
            if (tenantId != Guid.Empty)
            {
                _logger.SettingTenantIdBasedOnSourceIdentifierAndStrategy(
                    tenantId,
                    sourceIdentifier,
                    _config.TenantResolution.Strategy.ToString());
                return tenantId;
            }
        }

        _logger.AttemptingToResolveUsingHost(request.Host.Host);
        var hostResolvedTenantId = _config.Tenants.FirstOrDefault(_ => _.Value.Domain.Equals(request.Host.Host)).Key;
        if (hostResolvedTenantId != Guid.Empty)
        {
            _logger.SettingTenantIdBasedOnConfiguredHost(hostResolvedTenantId, request.Host.Host);
            return hostResolvedTenantId;
        }

        _logger.TenantIdNotResolved();
        return TenantId.NotSet;
    }
}
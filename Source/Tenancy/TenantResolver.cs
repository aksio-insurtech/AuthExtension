// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="ITenantResolver"/>.
/// </summary>
public class TenantResolver : ITenantResolver
{
    readonly Config _config;
    readonly ISourceIdentifierResolver _resolver;
    readonly ILogger<TenantResolver> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TenantResolver"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="resolver">The <see cref="ISourceIdentifierResolver"/> to use.</param>
    /// <param name="logger">Logger for logging.</param>
    public TenantResolver(Config config, ISourceIdentifierResolver resolver, ILogger<TenantResolver> logger)
    {
        _config = config;
        _resolver = resolver;
        _logger = logger;
    }

    /// <inheritdoc/>
    public bool TryResolve(HttpRequest request, out TenantId tenantId)
    {
        // Get the source identifier, using the configured strategies.
        if (!_resolver.TryResolve(_config, request, out var sourceIdentifier))
        {
            // Logged by source identifier resolver.
            tenantId = TenantId.NotSet;
            return false;
        }

        // string.Empty signifies that no tenant lookup was done, but it is acceptable (used by NoneSourceIdentifierResolver).
        if (sourceIdentifier.Length == 0)
        {
            _logger.SourceIdentifierEmptyUsingTenantIdNotSet();
            tenantId = TenantId.NotSet;
            return true;
        }

        // If we got a source identifier, find the appropriate tenantid for this identifier.
        _logger.AttemptingToResolveUsingSourceIdentifier(sourceIdentifier);
        tenantId = _config.Tenants.FirstOrDefault(_ => _.Value.SourceIdentifiers.Any(t => t == sourceIdentifier)).Key;
        if (tenantId == TenantId.NotSet)
        {
            _logger.TenantIdNotResolved(sourceIdentifier);
            tenantId = Guid.Empty;
            return false;
        }

        _logger.SettingTenantIdBasedOnSourceIdentifierAndStrategy(
            tenantId,
            sourceIdentifier,
            _config.TenantResolutions.Select(r => r.Strategy.ToString()));
        return true;
    }
}
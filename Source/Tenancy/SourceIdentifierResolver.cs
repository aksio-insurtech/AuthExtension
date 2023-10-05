// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

namespace Aksio.IngressMiddleware.Tenancy;

/// <inheritdoc/>
public class SourceIdentifierResolver : ISourceIdentifierResolver
{
    readonly ILogger<SourceIdentifierResolver> _logger;
    readonly Dictionary<TenantSourceIdentifierResolverType, ISourceIdentifier> _resolvers;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimsSourceIdentifier"/> class.
    /// </summary>
    /// <param name="resolvers">The list of available source identifiers.</param>
    /// <param name="logger">Logger for logging.</param>
    public SourceIdentifierResolver(IEnumerable<ISourceIdentifier> resolvers, ILogger<SourceIdentifierResolver> logger)
    {
        _resolvers = resolvers.ToDictionary(r => r.ResolverType, r => r);
        _logger = logger;
    }

    /// <inheritdoc/>
    public bool TryResolve(Config config, HttpRequest request, out string sourceIdentifier)
    {
        // Process the strategies in configured order.
        foreach (var strategy in config.TenantResolutions)
        {
            if (!_resolvers.ContainsKey(strategy.Strategy))
            {
                throw new TenantResolutionStrategyNotConfigured();
            }

            if (_resolvers[strategy.Strategy].TryResolve(strategy.Options, request, out sourceIdentifier))
            {
                _logger.ResolvedSourceIdentifierWithStrategy(sourceIdentifier, strategy.Strategy.ToString());
                return true;
            }
        }

        _logger.CouldNotResolveSourceIdentifierWithAnyConfiguredStrategy();
        sourceIdentifier = string.Empty;
        return false;
    }
}
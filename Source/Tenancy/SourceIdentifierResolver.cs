// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Data.SqlTypes;
using System.Text.Json;
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
    public string? Resolve(Config config, HttpRequest request)
    {
        // Process the strategies in configured order. 
        foreach (var strategy in config.TenantResolutions)
        {
            if (!_resolvers.ContainsKey(strategy.Strategy))
            {
                throw new TenantResolutionStrategyNotConfigured();
            }

            var sourceIdentifier = _resolvers[strategy.Strategy].Resolve(strategy.Options, request);
            if (sourceIdentifier != null)
            {
                _logger.ResolvedSourceIdentifierWithStrategy(sourceIdentifier, strategy.Strategy.ToString());
                return sourceIdentifier;
            }
        }

        return null;
    }
}
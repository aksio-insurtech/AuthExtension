// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

/// <summary>
/// Represents an implementation of <see cref="ISourceIdentifierResolver"/> that always resolves the tenant to a specific tenant.
/// </summary>
public class SpecifiedSourceIdentifier : ISourceIdentifier
{
    readonly ILogger<SpecifiedSourceIdentifier> _logger;

    /// <inheritdoc/>
    public TenantSourceIdentifierResolverType ResolverType => TenantSourceIdentifierResolverType.Specified;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecifiedSourceIdentifier"/> class.
    /// </summary>
    /// <param name="logger">Logger for logging.</param>
    public SpecifiedSourceIdentifier(ILogger<SpecifiedSourceIdentifier> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public bool TryResolve(JsonObject options, HttpRequest request, out string sourceIdentifier)
    {
        var config = options.Deserialize<SpecifiedSourceIdentifierOptions>(Globals.JsonSerializerOptions)!;
        if (string.IsNullOrWhiteSpace(config?.SourceIdentifier))
        {
            sourceIdentifier = string.Empty;
            return false;
        }

        _logger.SettingSourceIdentifierAsSpecified(config.SourceIdentifier);
        sourceIdentifier = config.SourceIdentifier;
        return true;
    }
}
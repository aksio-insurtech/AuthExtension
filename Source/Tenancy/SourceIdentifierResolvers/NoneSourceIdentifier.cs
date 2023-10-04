// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

/// <summary>
/// Represents an implementation of <see cref="ITenantResolver"/> that always resolves the tenant to NotSet.
/// </summary>
public class NoneSourceIdentifier : ISourceIdentifier
{
    readonly ILogger<NoneSourceIdentifier> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="NoneSourceIdentifier"/> class.
    /// </summary>
    /// <param name="logger">Logger for logging.</param>
    public NoneSourceIdentifier(ILogger<NoneSourceIdentifier> logger)
    {
        _logger = logger;
    }
    
    /// <inheritdoc/>
    public TenantSourceIdentifierResolverType ResolverType => TenantSourceIdentifierResolverType.None;

    /// <inheritdoc/>
    public string Resolve(JsonObject options, HttpRequest request)
    {
        _logger.SettingSourceIdentifierToEmpty();
        return string.Empty;
    }
}
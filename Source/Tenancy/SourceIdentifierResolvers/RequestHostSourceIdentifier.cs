// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

/// <summary>
/// Represents an implementation of <see cref="ISourceIdentifierResolver"/> that always resolves the tenant to a specific tenant.
/// </summary>
public class RequestHostSourceIdentifier : ISourceIdentifier
{
    readonly ILogger<RequestHostSourceIdentifier> _logger;

    /// <inheritdoc/>
    public TenantSourceIdentifierResolverType ResolverType => TenantSourceIdentifierResolverType.Host;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestHostSourceIdentifier"/> class.
    /// </summary>
    /// <param name="logger">Logger for logging.</param>
    public RequestHostSourceIdentifier(ILogger<RequestHostSourceIdentifier> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public string? Resolve(JsonObject options, HttpRequest request)
    {
        var config = options.Deserialize<RequestHostSourceIdentifierOptions>(Globals.JsonSerializerOptions)!;

        foreach (var configuredHost in config.Hostnames)
        {
            if (configuredHost.Key.Equals(request.Host.Host, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.SettingSourceIdentifierBasedOnConfiguredHost(configuredHost.Value, request.Host.Host);
                return configuredHost.Value;
            }
        }

        _logger.HostNotMatched();
        return null;
    }
}
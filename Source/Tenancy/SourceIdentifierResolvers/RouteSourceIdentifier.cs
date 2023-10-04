// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

#pragma warning disable MA0009 // We trust RegEx coming from configuration not to be a potential DOS attack

/// <summary>
/// Represents a source identifier resolver for routes.
/// </summary>
public class RouteSourceIdentifier : ISourceIdentifier
{
    const string SourceIdentifier = "sourceIdentifier";

    readonly IDictionary<string, Regex> _regularExpressions = new Dictionary<string, Regex>();
    readonly ILogger<RouteSourceIdentifier> _logger;

    /// <inheritdoc/>
    public TenantSourceIdentifierResolverType ResolverType => TenantSourceIdentifierResolverType.Route;

    /// <summary>
    /// Initializes a new instance of the <see cref="RouteSourceIdentifier"/> class.
    /// </summary>
    /// <param name="logger">Logger for logging.</param>
    public RouteSourceIdentifier(ILogger<RouteSourceIdentifier> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public string? Resolve(JsonObject options, HttpRequest request)
    {
        var config = options.Deserialize<RouteSourceIdentifierOptions>(Globals.JsonSerializerOptions)!;

        var originalUri = request.Headers[Headers.OriginalUri].FirstOrDefault() ?? string.Empty;

        // TODO: this needs a revisit, as this log-message might end up containing sensitive information.
        _logger.ResolvingUsingOriginalUri(originalUri);

        if (!_regularExpressions.ContainsKey(config.RegularExpression))
        {
            _regularExpressions[config.RegularExpression] = new(
                config.RegularExpression,
                RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        }

        var regex = _regularExpressions[config.RegularExpression];
        var match = regex.Match(originalUri);
        if (match.Success && match.Groups.ContainsKey(SourceIdentifier))
        {
            _logger.RouteMatched();
            var value = match.Groups[SourceIdentifier].Value;
            if (!string.IsNullOrEmpty(value))
            {
                _logger.SourceIdentifierMatched(value);
                return value;
            }
        }

        _logger.RouteNotMatched();
        return null;
    }
}
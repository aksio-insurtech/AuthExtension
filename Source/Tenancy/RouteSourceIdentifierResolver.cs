// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

#pragma warning disable MA0009 // We trust RegEx coming from configuration not to be a potential DOS attack

/// <summary>
/// Represents a source identifier resolver for routes.
/// </summary>
public class RouteSourceIdentifierResolver : TenantSourceIdentifierResolver, ITenantSourceIdentifierResolver<RouteSourceIdentifierResolverOptions>
{
    const string SourceIdentifier = "sourceIdentifier";

    readonly IDictionary<string, Regex> _regularExpressions = new Dictionary<string, Regex>();
    readonly ILogger<RouteSourceIdentifierResolver> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RouteSourceIdentifierResolver"/> class.
    /// </summary>
    /// <param name="logger">Logger for logging.</param>
    public RouteSourceIdentifierResolver(ILogger<RouteSourceIdentifierResolver> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public Task<string> Resolve(Config config, RouteSourceIdentifierResolverOptions options, HttpRequest request)
    {
        var originalUri = request.Headers[Headers.OriginalUri].FirstOrDefault() ?? string.Empty;
        _logger.ResolvingUsingOriginalUri(originalUri);

        if (!_regularExpressions.ContainsKey(options.RegularExpression))
        {
            _regularExpressions[options.RegularExpression] = new Regex(options.RegularExpression, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        }

        var regex = _regularExpressions[options.RegularExpression];
        var match = regex.Match(originalUri);
        if (match.Success && match.Groups.ContainsKey(SourceIdentifier))
        {
            _logger.RouteMatched();
            var value = match.Groups[SourceIdentifier].Value;
            if (!string.IsNullOrEmpty(value))
            {
                _logger.SourceIdentifierMatched(value);
                return Task.FromResult(value);
            }
        }

        _logger.RouteNotMatched();
        return Task.FromResult(string.Empty);
    }
}

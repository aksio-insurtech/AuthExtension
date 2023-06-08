// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;
using Aksio.Cratis.Execution;
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

    /// <inheritdoc/>
    public Task<TenantId> Resolve(Config config, RouteSourceIdentifierResolverOptions options, HttpRequest request)
    {
        var originalUri = request.Headers[Headers.OriginalUri].FirstOrDefault() ?? string.Empty;

        if (!_regularExpressions.ContainsKey(options.RegularExpression))
        {
            _regularExpressions[options.RegularExpression] = new Regex(options.RegularExpression, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        }

        var regex = _regularExpressions[options.RegularExpression];
        var match = regex.Match(originalUri);
        if (match.Success && match.Groups.ContainsKey(SourceIdentifier))
        {
            var value = match.Groups[SourceIdentifier].Value;
            if (!string.IsNullOrEmpty(value))
            {
                return Task.FromResult(new TenantId(Guid.Parse(value)));
            }
        }

        return Task.FromResult(TenantId.NotSet);
    }
}

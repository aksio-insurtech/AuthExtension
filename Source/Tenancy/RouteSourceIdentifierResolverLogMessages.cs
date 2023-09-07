// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy;

static partial class RouteSourceIdentifierResolverLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Resolving tenant from route using original URI: {originalUri}")]
    internal static partial void ResolvingUsingOriginalUri(this ILogger<RouteSourceIdentifierResolver> logger, string originalUri);

    [LoggerMessage(1, LogLevel.Information, "Route matched")]
    internal static partial void RouteMatched(this ILogger<RouteSourceIdentifierResolver> logger);

    [LoggerMessage(2, LogLevel.Information, "Source identifier '{SourceIdentifier}' matched from route")]
    internal static partial void SourceIdentifierMatched(this ILogger<RouteSourceIdentifierResolver> logger, string sourceIdentifier);

    [LoggerMessage(3, LogLevel.Information, "Route not matched")]
    internal static partial void RouteNotMatched(this ILogger<RouteSourceIdentifierResolver> logger);
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

static partial class RouteSourceIdentifierLogMessages
{
    [LoggerMessage(0, LogLevel.Debug, "Resolving tenant from route using original URI: {originalUri}")]
    internal static partial void ResolvingUsingOriginalUri(this ILogger<RouteSourceIdentifier> logger, string originalUri);

    [LoggerMessage(1, LogLevel.Debug, "Route matched")]
    internal static partial void RouteMatched(this ILogger<RouteSourceIdentifier> logger);

    [LoggerMessage(2, LogLevel.Information, "Source identifier '{SourceIdentifier}' matched from route")]
    internal static partial void SourceIdentifierMatched(this ILogger<RouteSourceIdentifier> logger, string sourceIdentifier);

    [LoggerMessage(3, LogLevel.Debug, "Route not matched")]
    internal static partial void RouteNotMatched(this ILogger<RouteSourceIdentifier> logger);
}
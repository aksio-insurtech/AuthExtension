// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// SourceIdentifierResolver log messages.
/// </summary>
public static partial class SourceIdentifierResolverLogMessages
{
    [LoggerMessage(
        0,
        LogLevel.Debug,
        "Setting source identifier to '{SourceIdentifier}' based on strategy {Strategy}")]
    internal static partial void ResolvedSourceIdentifierWithStrategy(
        this ILogger<SourceIdentifierResolver> logger,
        string sourceIdentifier, string strategy);

    [LoggerMessage(
        1,
        LogLevel.Information,
        "Could not resolve source identifier with any configured strategy.")]
    internal static partial void CouldNotResolveSourceIdentifierWithAnyConfiguredStrategy(
        this ILogger<SourceIdentifierResolver> logger);
}
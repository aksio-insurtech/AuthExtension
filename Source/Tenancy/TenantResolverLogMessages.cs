// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;

namespace Aksio.IngressMiddleware.Tenancy;

static partial class TenantResolverLogMessages
{
    [LoggerMessage(0, LogLevel.Debug, "Attempting to resolve tenant using source identifier {SourceIdentifier}")]
    internal static partial void AttemptingToResolveUsingSourceIdentifier(this ILogger<TenantResolver> logger, string sourceIdentifier);

    [LoggerMessage(
        1,
        LogLevel.Information,
        "Setting tenant id to '{TenantId}' based on source identifier ({SourceIdentifier}) resolved using strategies {Strategies}")]
    internal static partial void SettingTenantIdBasedOnSourceIdentifierAndStrategy(
        this ILogger<TenantResolver> logger,
        TenantId tenantId,
        string sourceIdentifier,
        IEnumerable<string> strategies);

    [LoggerMessage(4, LogLevel.Information, "TenantId is not resolved. Source identifier was {SourceIdentifier}")]
    internal static partial void TenantIdNotResolved(this ILogger<TenantResolver> logger, string sourceIdentifier);

    [LoggerMessage(5, LogLevel.Information, "Source identifier is empty, setting tenant id to NotSet")]
    internal static partial void SourceIdentifierEmptyUsingTenantIdNotSet(this ILogger<TenantResolver> logger);
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;

namespace Aksio.IngressMiddleware.Tenancy;

static partial class TenantResolverLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Attempting to resolve tenant using source identifier")]
    internal static partial void AttemptingToResolveUsingSourceIdentifier(this ILogger<TenantResolver> logger);

    [LoggerMessage(
        1,
        LogLevel.Information,
        "Setting tenant id to '{TenantId}' based on source identifier ({SourceIdentifier}) resolved using {Strategy}")]
    internal static partial void SettingTenantIdBasedOnSourceIdentifierAndStrategy(
        this ILogger<TenantResolver> logger,
        TenantId tenantId,
        string sourceIdentifier,
        string strategy);

    [LoggerMessage(2, LogLevel.Information, "Attempting to resolve tenant using host '{Host}'")]
    internal static partial void AttemptingToResolveUsingHost(this ILogger<TenantResolver> logger, string host);

    [LoggerMessage(3, LogLevel.Information, "Setting tenant id to '{TenantId}' based on configured host ({Host})")]
    internal static partial void SettingTenantIdBasedOnConfiguredHost(
        this ILogger<TenantResolver> logger,
        TenantId tenantId,
        string host);

    [LoggerMessage(4, LogLevel.Information, "TenantId is not resolved, setting to empty")]
    internal static partial void TenantIdNotResolved(this ILogger<TenantResolver> logger);
}
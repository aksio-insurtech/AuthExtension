// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

/// <summary>
/// ClaimsSourceIdentifier log messages.
/// </summary>
public static partial class ClaimsSourceIdentifierLogMessages
{
    [LoggerMessage(
        0,
        LogLevel.Information,
        "Setting source identifier to '{SourceIdentifier}' based on configured principal claim tenant id")]
    internal static partial void SettingSourceIdentifierBasedOnTenantClaim(
        this ILogger<ClaimsSourceIdentifier> logger,
        string sourceIdentifier);

    [LoggerMessage(1, LogLevel.Debug, "TenantId claim did not match any source identifiers")]
    internal static partial void TenantClaimNotMatched(this ILogger<ClaimsSourceIdentifier> logger);
}
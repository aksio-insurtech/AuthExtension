// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

/// <summary>
/// RequestHostSourceIdentifier log messages.
/// </summary>
public static partial class RequestHostSourceIdentifierLogMessages
{
    [LoggerMessage(
        0,
        LogLevel.Information,
        "Setting source identifier to '{SourceIdentifier}' based on configured host ({Host})")]
    internal static partial void SettingSourceIdentifierBasedOnConfiguredHost(
        this ILogger<RequestHostSourceIdentifier> logger,
        string sourceIdentifier,
        string host);

    [LoggerMessage(1, LogLevel.Debug, "Host not matched")]
    internal static partial void HostNotMatched(this ILogger<RequestHostSourceIdentifier> logger);
}
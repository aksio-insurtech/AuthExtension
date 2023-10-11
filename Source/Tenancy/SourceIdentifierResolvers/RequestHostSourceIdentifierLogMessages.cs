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
        "Setting source identifier to '{SourceIdentifier}' based on request host ({RequestHost})")]
    internal static partial void SettingSourceIdentifierBasedOnConfiguredHost(
        this ILogger<RequestHostSourceIdentifier> logger,
        string sourceIdentifier,
        string requestHost);

    [LoggerMessage(1, LogLevel.Debug, "Request host {RequestHost} did not match any configured hosts.")]
    internal static partial void HostNotMatched(this ILogger<RequestHostSourceIdentifier> logger, string requestHost);
}
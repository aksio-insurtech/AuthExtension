// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

/// <summary>
/// NoneSourceIdentifier log message.
/// </summary>
public static partial class NoneSourceIdentifierLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Setting source identifier to empty")]
    internal static partial void SettingSourceIdentifierToEmpty(this ILogger<NoneSourceIdentifier> logger);
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

public static partial class SpecifiedSourceIdentifierLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Setting source identifier to specified identifier '{SourceIdentifier}'")]
    internal static partial void SettingSourceIdentifierAsSpecified(
        this ILogger<SpecifiedSourceIdentifier> logger,
        string sourceIdentifier);
}
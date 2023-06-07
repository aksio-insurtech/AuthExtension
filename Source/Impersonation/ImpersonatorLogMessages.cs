// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Impersonation;

internal static partial class ImpersonatorLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Impersonation not authorized for principal {PrincipalId} ({PrincipalName})")]
    internal static partial void ImpersonationNotAuthorized(this ILogger<Impersonator> logger, string principalId, string principalName);

    [LoggerMessage(1, LogLevel.Information, "Impersonation authorized for principal {PrincipalId} ({PrincipalName})")]
    internal static partial void ImpersonationAuthorized(this ILogger<Impersonator> logger, string principalId, string principalName);

    [LoggerMessage(2, LogLevel.Information, "Performing impersonation for principal {PrincipalId} ({PrincipalName})")]
    internal static partial void PerformingImpersonation(this ILogger<Impersonator> logger, string principalId, string principalName);
}
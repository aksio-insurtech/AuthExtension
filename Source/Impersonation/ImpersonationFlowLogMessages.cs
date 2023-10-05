// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Impersonation;

static partial class ImpersonationFlowLogMessages
{
    [LoggerMessage(0, LogLevel.Debug, "Checking if request should be impersonated. IsImpersonateRoute: {IsImpersonateRoute}, HasPrincipal: {HasPrincipal}")]
    internal static partial void CheckingIfRequestShouldBeImpersonated(this ILogger<ImpersonationFlow> logger, bool isImpersonateRoute, bool hasPrincipal);

    [LoggerMessage(1, LogLevel.Information, "Request should be impersonated. Principal id {PrincipalId}, using identity provider {IdentityProvider}")]
    internal static partial void ShouldImpersonate(this ILogger<ImpersonationFlow> logger, string principalId, string identityProvider);

    [LoggerMessage(2, LogLevel.Debug, "Request should not be impersonated")]
    internal static partial void ShouldNotImpersonate(this ILogger<ImpersonationFlow> logger);
}

// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.EntraIdRoles;

static partial class EntraIdRolesLogMessages
{
    [LoggerMessage(
        0,
        LogLevel.Warning,
        "Client request did not include a client principal, will not authorize. Client address = {ClientIp}")]
    internal static partial void ClientDidNotProvidePrincipal(this ILogger<EntraIdRoles> logger, string clientIp);

    [LoggerMessage(
        1,
        LogLevel.Warning,
        "Client did not have any accepted roles. User had roles: {UserRoles}. Principal id {PrincipalId}, lient address = {ClientIp}")]
    internal static partial void UserDidNotHaveAnyMatchingRoles(
        this ILogger<EntraIdRoles> logger,
        string principalId,
        List<string> userRoles,
        string clientIp);

    [LoggerMessage(
        2,
        LogLevel.Information,
        "Client logged in with role(s) {AcceptedRoles}. Principal id {PrincipalId}, lient address = {ClientIp}")]
    internal static partial void UserLoggedInWithRoles(
        this ILogger<EntraIdRoles> logger,
        string principalId,
        List<string> acceptedRoles,
        string clientIp);

    [LoggerMessage(
        3,
        LogLevel.Information,
        "Client logged in with without roles. Principal id {PrincipalId}, client address = {ClientIp}")]
    internal static partial void AcceptingClientWithoutRole(
        this ILogger<EntraIdRoles> logger,
        string principalId,
        string clientIp);
}
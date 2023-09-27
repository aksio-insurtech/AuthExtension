// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.RoleAuthorization;

static partial class RoleAuthorizerLogMessages
{
    [LoggerMessage(
        0,
        LogLevel.Warning,
        "Client request did not include a client principal, will not authorize. Client address = {ClientIp}")]
    internal static partial void ClientDidNotProvidePrincipal(this ILogger<RoleAuthorizer> logger, string clientIp);

    [LoggerMessage(
        1,
        LogLevel.Warning,
        "Client did not have any accepted roles. User had roles: {UserRoles}. Principal id {PrincipalId}, client address = {ClientIp}")]
    internal static partial void UserDidNotHaveAnyMatchingRoles(
        this ILogger<RoleAuthorizer> logger,
        string principalId,
        IReadOnlyList<string> userRoles,
        string clientIp);

    [LoggerMessage(
        2,
        LogLevel.Information,
        "Client logged in with role(s) {AcceptedRoles}. Principal id {PrincipalId}, client address = {ClientIp}")]
    internal static partial void UserLoggedInWithRoles(
        this ILogger<RoleAuthorizer> logger,
        string principalId,
        IReadOnlyList<string> acceptedRoles,
        string clientIp);

    [LoggerMessage(
        3,
        LogLevel.Information,
        "Client logged in with without roles. Principal id {PrincipalId}, client address = {ClientIp}")]
    internal static partial void AcceptingClientWithoutRole(
        this ILogger<RoleAuthorizer> logger,
        string principalId,
        string clientIp);

    [LoggerMessage(
        4,
        LogLevel.Warning,
        "Client logged in with principal without audience claim ('aud'). Principal id {PrincipalId}, client address = {ClientIp}")]
    internal static partial void ClientPrincipalDidNotHaveAudienceClaim(
        this ILogger<RoleAuthorizer> logger,
        string principalId,
        string clientIp);

    [LoggerMessage(
        5,
        LogLevel.Warning,
        "Client logged in with an unknown audience claim, aud={Audience}. Principal id {PrincipalId}, client address = {ClientIp}")]
    internal static partial void ClientPrincipalAudienceIsNotConfigured(
        this ILogger<RoleAuthorizer> logger,
        string audience,
        string principalId,
        string clientIp);
}
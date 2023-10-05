// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.BearerTokens;

static partial class OAuthBearerTokensLogMessages
{
    [LoggerMessage(0, LogLevel.Warning, "OAuth bearer token is missing the 'Authorization' header. Client address = {ClientIp}")]
    internal static partial void MissingAuthorizationHeader(this ILogger<OAuthBearerTokens> logger, string clientIp);

    [LoggerMessage(1, LogLevel.Warning, "OAuth bearer missing in the 'Authorization' header. Client address = {ClientIp}")]
    internal static partial void MissingBearerInHeader(this ILogger<OAuthBearerTokens> logger, string clientIp);

    [LoggerMessage(2, LogLevel.Warning, "OAuth token expired at {Expires}. Client address = {ClientIp}")]
    internal static partial void TokenExpired(this ILogger<OAuthBearerTokens> logger, DateTime expires, string clientIp);

    [LoggerMessage(3, LogLevel.Warning, "OAuth token validation failed. Client address = {ClientIp}")]
    internal static partial void CouldNotValidateToken(this ILogger<OAuthBearerTokens> logger, Exception exception, string clientIp);

    [LoggerMessage(4, LogLevel.Warning, "Unauthorized: {Message} - {Description}. Client address = {ClientIp}")]
    internal static partial void Unauthorized(this ILogger<OAuthBearerTokens> logger, string message, string description, string clientIp);

    [LoggerMessage(5, LogLevel.Debug, "OAuth bearer tokens validation is not enabled, skipping validation")]
    internal static partial void OAuthBearerTokensValidationNotEnabled(this ILogger<OAuthBearerTokens> logger);

    [LoggerMessage(6, LogLevel.Error, "OAuth cannot refresh json webkey set! Client address = {ClientIp}")]
    internal static partial void CannotRefreshJsonWebkeySet(this ILogger<OAuthBearerTokens> logger, string clientIp);

    [LoggerMessage(7, LogLevel.Warning, "OAuth received invalid token from client address = {ClientIp}. Token payload was: {TokenPayload}")]
    internal static partial void InvalidToken(this ILogger<OAuthBearerTokens> logger, string tokenPayload, string clientIp);

    [LoggerMessage(8, LogLevel.Information, "OAuth accepted login from client address = {ClientIp} for token payload {TokenPayload}")]
    internal static partial void LoggedInWithToken(this ILogger<OAuthBearerTokens> logger, string tokenPayload, string clientIp);
}
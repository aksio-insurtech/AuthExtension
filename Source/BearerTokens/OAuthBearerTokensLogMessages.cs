// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.BearerTokens;

internal static partial class OAuthBearerTokensLogMessages
{
    [LoggerMessage(0, LogLevel.Error, "OAuth bearer tokens is missing the 'Authorization' header")]
    internal static partial void MissingHeader(this ILogger<OAuthBearerTokens> logger);

    [LoggerMessage(1, LogLevel.Error, "Missing bearer in the 'Authorization' header")]
    internal static partial void MissingBearerInHeader(this ILogger<OAuthBearerTokens> logger);

    [LoggerMessage(2, LogLevel.Error, "Invalid token : '{Token}'")]
    internal static partial void InvalidToken(this ILogger<OAuthBearerTokens> logger, string token);

    [LoggerMessage(3, LogLevel.Error, "Token expired at {Expires}")]
    internal static partial void TokenExpired(this ILogger<OAuthBearerTokens> logger, DateTime Expires);

    [LoggerMessage(4, LogLevel.Error, "Could not validate token")]
    internal static partial void CouldNotValidateToken(this ILogger<OAuthBearerTokens> logger, Exception exception);

    [LoggerMessage(5, LogLevel.Error, "Could not get well-known authority document")]
    internal static partial void CouldNotGetWellKnownAuthorityDocument(this ILogger<OAuthBearerTokens> logger);

    [LoggerMessage(6, LogLevel.Error, "Could not parse the well-known authority document")]
    internal static partial void CouldNotParseTheWellKnownAuthorityDocument(this ILogger<OAuthBearerTokens> logger);

    [LoggerMessage(7, LogLevel.Error, "Could not get JWKS document")]
    internal static partial void CouldNotGetJWKSDocument(this ILogger<OAuthBearerTokens> logger);

    [LoggerMessage(8, LogLevel.Error, "Unauthorized: {Message} - {Description}")]
    internal static partial void Unauthorized(this ILogger<OAuthBearerTokens> logger, string Message, string Description);

    [LoggerMessage(9, LogLevel.Information, "OAuth bearer tokens validation is not enabled, skipping validation")]
    internal static partial void OAuthBearerTokensValidationNotEnabled(this ILogger<OAuthBearerTokens> logger);
}
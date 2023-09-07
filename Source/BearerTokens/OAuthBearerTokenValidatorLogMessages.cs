// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.BearerTokens;

static partial class OAuthBearerTokenValidatorLogMessages
{
    [LoggerMessage(0, LogLevel.Error, "Could not get well-known authority document")]
    internal static partial void CouldNotGetWellKnownAuthorityDocument(this ILogger<OAuthBearerTokenValidator> logger);

    [LoggerMessage(1, LogLevel.Error, "Could not parse the well-known authority document")]
    internal static partial void CouldNotParseTheWellKnownAuthorityDocument(this ILogger<OAuthBearerTokenValidator> logger);

    [LoggerMessage(2, LogLevel.Error, "Could not get JWKS document")]
    internal static partial void CouldNotGetJWKSDocument(this ILogger<OAuthBearerTokenValidator> logger);

    [LoggerMessage(3, LogLevel.Error, "Invalid token : '{Token}'")]
    internal static partial void InvalidToken(this ILogger<OAuthBearerTokenValidator> logger, string token);
}
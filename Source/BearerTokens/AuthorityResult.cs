// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.BearerTokens;

#pragma warning disable CA1707 // Identifiers should not contain underscores

/// <summary>
/// Represents the result of an authority discovery.
/// </summary>
/// <param name="issuer">The issuer for the authority.</param>
/// <param name="jwks_uri">The URI for the JWKS.</param>
public record AuthorityResult(string issuer, string jwks_uri);
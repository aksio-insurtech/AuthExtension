// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Aksio.IngressMiddleware.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Aksio.IngressMiddleware.BearerTokens;

#pragma warning disable CA2000 // We are not responsible for disposing the IHttpClientFactory

/// <inheritdoc/>
public class OAuthBearerTokenValidator : IOAuthBearerTokenValidator
{
    static AuthorityResult? _authority;
    static JsonWebKeySet? _jwks;
    static DateTime _jwksLastUpdated = DateTime.MinValue;
    readonly Config _config;
    readonly IHttpClientFactory _httpClientFactory;
    readonly ILogger<OAuthBearerTokenValidator> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthBearerTokenValidator"/> class.
    /// </summary>
    /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/> to use to create clients.</param>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="logger">Logger for logging.</param>
    public OAuthBearerTokenValidator(
        IHttpClientFactory httpClientFactory,
        Config config,
        ILogger<OAuthBearerTokenValidator> logger)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<bool> RefreshJwks()
    {
        if (_jwks is not null && _jwksLastUpdated > DateTime.UtcNow.AddHours(-5))
        {
            // No need to refresh the web keys at this time.
            return true;
        }

        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(_config.OAuthBearerTokens.Authority);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            _logger.CouldNotGetWellKnownAuthorityDocument();
            return false;
        }

        _authority = await response.Content.ReadFromJsonAsync<AuthorityResult>();
        if (_authority is null)
        {
            _logger.CouldNotParseTheWellKnownAuthorityDocument();
            return false;
        }

        response = await client.GetAsync(_authority.jwks_uri);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            _logger.CouldNotGetJWKSDocument();
        }

        var jwks = await response.Content.ReadAsStringAsync();
        _jwks = JsonWebKeySet.Create(jwks);

        _jwksLastUpdated = DateTime.UtcNow;

        return true;
    }

    /// <inheritdoc/>
    public bool ValidateToken(string token, out JwtSecurityToken? jwtToken)
    {
        jwtToken = null;
        if (_jwks is null || _authority is null)
        {
            return false;
        }

        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = _jwks.Keys[0],

#pragma warning disable CA5404// Disable audience validation
            ValidateAudience = false,
            ValidIssuer = _authority.issuer
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        jwtToken = tokenHandler.ReadJwtToken(token);
        tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
        if (validatedToken is null)
        {
            _logger.InvalidToken(jwtToken.EncodedPayload);
            return false;
        }

        return true;
    }
}
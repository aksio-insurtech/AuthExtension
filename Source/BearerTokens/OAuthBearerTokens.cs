// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;
using Aksio.Cratis.Execution;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Security;
using Microsoft.IdentityModel.Tokens;

namespace Aksio.IngressMiddleware.BearerTokens;

/// <summary>
/// Represents an implementation of <see cref="IOAuthBearerTokens"/>.
/// </summary>
public class OAuthBearerTokens : IOAuthBearerTokens
{
    static AuthorityResult? _authority;
    static JsonWebKeySet? _jwks;
    static DateTime _jwksLastUpdated = DateTime.MinValue;
    readonly Config _config;
    readonly IHttpClientFactory _httpClientFactory;
    readonly ILogger<OAuthBearerTokens> _logger;
    readonly IActionResult _ok = new OkResult();

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthBearerTokens"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/> to use to create clients.</param>
    /// <param name="logger">Logger for logging.</param>
    public OAuthBearerTokens(
        Config config,
        IHttpClientFactory httpClientFactory,
        ILogger<OAuthBearerTokens> logger)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IActionResult> Handle(HttpRequest request, HttpResponse response, TenantId tenantId)
    {
        if (!_config.OAuthBearerTokens.IsEnabled)
        {
            _logger.LogInformation("OAuth bearer tokens validation is not enabled, skipping validation");
            return _ok;
        }

        if (request.Headers.Authorization.Count == 0)
        {
            return Unauthorized(response, "Missing header", "Authorization header missing");
        }

        var strings = request.Headers.Authorization.ToString().Split(' ');
        if (strings.Length < 2 && strings[0].StartsWith("Bearer"))
        {
            return Unauthorized(response, "Missing Bearer in header", "Authorization header missing 'Bearer' in token value, unauthorized, Should be in the format \"Bearer {token}\"");
        }
        var token = strings[1];

        if (_jwks is null || _jwksLastUpdated < DateTime.UtcNow.AddHours(-5))
        {
            if (!await RefreshJwks())
            {
                return _ok;
            }
        }

        if (_jwks is null || _authority is null) return _ok;

        var jwk = _jwks.Keys[0];
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = jwk,

#pragma warning disable CA5404 // Disable audience validation
            ValidateAudience = false,
            ValidIssuer = _authority.issuer
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var jwtToken = tokenHandler.ReadJwtToken(token);
            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var valid = validatedToken != null;
            if (!valid)
            {
                return Unauthorized(response, "invalid_token", "Given token is invalid");
            }
            AddPrincipalHeader(response, jwtToken);
        }
        catch (SecurityTokenExpiredException ex)
        {
            return Unauthorized(response, "token_expired", $"Token has expired, it expired at {ex.Expires} (UTC)");
        }
        catch (Exception ex)
        {
            return Unauthorized(response, "invalid_token", $"Could not validate token ; {ex.Message}");
        }

        return _ok;
    }

    static void AddPrincipalHeader(HttpResponse response, JwtSecurityToken jwtToken)
    {
        var principal = jwtToken.ToClientPrincipal();
        var principalAsJson = JsonSerializer.Serialize(principal, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var principalAsJsonBytes = Encoding.UTF8.GetBytes(principalAsJson);
        response.Headers[Headers.Principal] = Convert.ToBase64String(principalAsJsonBytes);
    }

    async Task<bool> RefreshJwks()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(_config.OAuthBearerTokens.Authority);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogError("Could not get the well-known authority document");
            return false;
        }

        _authority = await response.Content.ReadFromJsonAsync<AuthorityResult>();
        if (_authority is null)
        {
            _logger.LogError("Could not parse the well-known authority document");
            return false;
        }

        response = await client.GetAsync(_authority.jwks_uri);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogError("Could not get JWKS document");
        }

        var jwks = await response.Content.ReadAsStringAsync();
        _jwks = JsonWebKeySet.Create(jwks);
        _jwksLastUpdated = DateTime.UtcNow;

        return true;
    }

    IActionResult Unauthorized(HttpResponse response, string message, string description)
    {
        response.Headers.Add("WWW-Authenticate", $"Bearer, error=\"{message}\", error_description=\"{description}\"");
        _logger.LogError(message);
        return new UnauthorizedResult();
    }
}
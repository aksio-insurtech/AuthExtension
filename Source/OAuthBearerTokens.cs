// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Aksio.Cratis.Execution;
using Microsoft.IdentityModel.Tokens;

namespace Aksio.IngressMiddleware;

public class OAuthBearerTokens
{
    static AuthorityResult? _authority;
    static JsonWebKeySet? _jwks;
    static DateTime _jwksLastUpdated = DateTime.MinValue;

    public static async Task HandleRequest(Config config, HttpRequest request, HttpResponse response, TenantId tenantId, IHttpClientFactory httpClientFactory)
    {
        if (!config.OAuthBearerTokens.IsEnabled)
        {
            Globals.Logger.LogInformation("OAuth bearer tokens validation is not enabled, skipping validation");
            return;
        }

        if (request.Headers.Authorization.Count == 0)
        {
            await Unauthorized(response, "Missing header", "Authorization header missing");
            return;
        }

        var strings = request.Headers.Authorization.ToString().Split(' ');
        if (strings.Length < 2 && strings[0].StartsWith("Bearer"))
        {
            await Unauthorized(response, "Missing Bearer in header", "Authorization header missing 'Bearer' in token value, unauthorized, Should be in the format \"Bearer {token}\"");
            return;
        }
        var token = strings[1];

        if (_jwks is null || _jwksLastUpdated < DateTime.UtcNow.AddHours(-5))
        {
            if (!await RefreshJwks(config, httpClientFactory))
            {
                return;
            }
        }

        if (_jwks is null || _authority is null) return;

        var jwk = _jwks.Keys.First();
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = jwk,
            ValidateAudience = false,
            ValidIssuer = _authority.issuer
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var valid = validatedToken != null;
            if (!valid)
            {
                await Unauthorized(response, "invalid_token", "Given token is invalid");
                return;
            }
        }
        catch (SecurityTokenExpiredException ex)
        {
            await Unauthorized(response, "token_expired", $"Token has expired, it expired at {ex.Expires} (UTC)");
            return;
        }
        catch (Exception ex)
        {
            await Unauthorized(response, "invalid_token", $"Could not validate token ; {ex.Message}");
            return;
        }
    }

    static async Task<bool> RefreshJwks(Config config, IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient();
        var response = await client.GetAsync(config.OAuthBearerTokens.Authority);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            Globals.Logger.LogError("Could not get the well-known authority document");
            return false;
        }

        _authority = await response.Content.ReadFromJsonAsync<AuthorityResult>();
        if (_authority is null)
        {
            Globals.Logger.LogError("Could not parse the well-known authority document");
            return false;
        }

        response = await client.GetAsync(_authority.jwks_uri);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            Globals.Logger.LogError("Could not get JWKS document");
        }

        var jwks = await response.Content.ReadAsStringAsync();
        _jwks = JsonWebKeySet.Create(jwks);
        _jwksLastUpdated = DateTime.UtcNow;

        return true;
    }

    static async Task Unauthorized(HttpResponse response, string message, string description)
    {
        response.StatusCode = 401;
        response.Headers.Add("WWW-Authenticate", $"Bearer, error=\"{message}\", error_description=\"{description}\"");
        Globals.Logger.LogError(message);
        await response.WriteAsync(message);
    }
}
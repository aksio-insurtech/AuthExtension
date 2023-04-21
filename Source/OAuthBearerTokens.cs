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
            var result = "Authorization header missing, unauthorized";
            response.StatusCode = 403;
            Globals.Logger.LogError(result);
            await response.WriteAsync(result);
            return;
        }

        var strings = request.Headers.Authorization.ToString().Split(' ');
        if (strings.Length < 2 && strings[0].StartsWith("Bearer"))
        {
            var result = "Authorization header missing 'Bearer' in token value, unauthorized, Should be in the format \"Bearer {token}\"";
            response.StatusCode = 403;
            Globals.Logger.LogError(result);
            await response.WriteAsync(result);
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
                response.StatusCode = 403;
                Globals.Logger.LogError("Invalid token");
                await response.WriteAsync("Invalid token");
                return;
            }
        }
        catch (SecurityTokenExpiredException ex)
        {
            response.StatusCode = 403;
            Globals.Logger.LogError("Token has expired", ex);
            await response.WriteAsync($"Token has expired, it expired at {ex.Expires}");
            return; 
        }
        catch (Exception ex)
        {
            response.StatusCode = 403;
            Globals.Logger.LogError("Could not validate token", ex);
            await response.WriteAsync($"Could not validate token ; {ex.Message}");
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
}
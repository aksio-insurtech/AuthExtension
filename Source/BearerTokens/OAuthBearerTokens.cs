// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Aksio.Execution;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Security;
using Microsoft.IdentityModel.Tokens;

namespace Aksio.IngressMiddleware.BearerTokens;

/// <summary>
/// Represents an implementation of <see cref="IOAuthBearerTokens"/>.
/// </summary>
public class OAuthBearerTokens : IOAuthBearerTokens
{
    readonly IOAuthBearerTokenValidator _oauthValidator;
    readonly Config _config;
    readonly ILogger<OAuthBearerTokens> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthBearerTokens"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="logger">Logger for logging.</param>
    /// <param name="oauthValidator">OAuth utils.</param>
    public OAuthBearerTokens(Config config, ILogger<OAuthBearerTokens> logger, IOAuthBearerTokenValidator oauthValidator)
    {
        _config = config;
        _logger = logger;
        _oauthValidator = oauthValidator;
    }

    /// <inheritdoc/>
    public bool IsEnabled() => _config.OAuthBearerTokens.IsEnabled;

    /// <inheritdoc/>
    public async Task<IActionResult> Handle(HttpRequest request, HttpResponse response, TenantId tenantId)
    {
        if (!_config.OAuthBearerTokens.IsEnabled)
        {
            // This should never occur, as IsEnabled() is called first.
            // If this happens, there is a bug in the code!
            _logger.OAuthBearerTokensValidationNotEnabled();
            return new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }

        // Get caller address, for logging purposes.
        var clientIp = request.Headers["X-Forwarded-For"].FirstOrDefault() ?? "(n/a)";

        if (request.Headers.Authorization.Count == 0)
        {
            _logger.MissingAuthorizationHeader(clientIp);
            return Unauthorized(response, "Missing header", "Authorization header missing");
        }

        var strings = request.Headers.Authorization.ToString()?.Split(' ');
        if (strings?.Length != 2 || !strings[0].StartsWith("Bearer"))
        {
            _logger.MissingBearerInHeader(clientIp);
            return Unauthorized(
                response,
                "Missing Bearer in header",
                "Authorization header missing 'Bearer' in token value, unauthorized, Should be in the format \"Bearer {token}\"");
        }

        var token = strings[1];

        if (!await _oauthValidator.RefreshJwks())
        {
            _logger.CannotRefreshJsonWebkeySet(clientIp);
            return Unauthorized(response, "OAuth authority problem", "Cannot refresh json webkey set");
        }

        try
        {
            if (!_oauthValidator.ValidateToken(token, out var jwtToken))
            {
                _logger.InvalidToken(token, clientIp);
                return Unauthorized(response, "invalid_token", "Given token is invalid");
            }

            AddPrincipalHeader(response, jwtToken!);
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.TokenExpired(ex.Expires, clientIp);
            return Unauthorized(response, "token_expired", $"Token has expired, it expired at {ex.Expires} (UTC)");
        }
        catch (Exception ex)
        {
            _logger.CouldNotValidateToken(ex, clientIp);
            return Unauthorized(response, "invalid_token", $"Could not validate token ; {ex.Message}");
        }

        _logger.LoggedInWithToken(token, clientIp);
        return new OkResult();
    }

    void AddPrincipalHeader(HttpResponse response, JwtSecurityToken jwtToken)
    {
        var principal = jwtToken.ToClientPrincipal().ToRawClientPrincipal();
        var principalAsJson = JsonSerializer.Serialize(
            principal,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var principalAsJsonBytes = Encoding.UTF8.GetBytes(principalAsJson);
        response.Headers[Headers.Principal] = Convert.ToBase64String(principalAsJsonBytes);
    }

    IActionResult Unauthorized(HttpResponse response, string message, string description)
    {
        response.Headers.Add("WWW-Authenticate", $"Bearer, error=\"{message}\", error_description=\"{description}\"");
        return new StatusCodeResult(StatusCodes.Status401Unauthorized);
    }
}
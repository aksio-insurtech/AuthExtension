// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Aksio.Cratis.Execution;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Identities;

/// <summary>
/// Represents an implementation of <see cref="IIdentityDetailsResolver"/>.
/// </summary>
public class IdentityDetailsResolver : IIdentityDetailsResolver
{
    readonly Config _config;
    readonly IHttpClientFactory _httpClientFactory;
    readonly ILogger<IdentityDetailsResolver> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityDetailsResolver"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> to use.</param>
    /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/> for creating clients.</param>
    /// <param name="logger">Logger for logging.</param>
    public IdentityDetailsResolver(
        Config config,
        IHttpClientFactory httpClientFactory,
        ILogger<IdentityDetailsResolver> logger)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<bool> Resolve(HttpRequest request, HttpResponse response, TenantId tenantId)
    {
        if (string.IsNullOrEmpty(_config.IdentityDetailsUrl))
        {
            _logger.LogInformation("Identity details url is not configured, skipping identity details resolution");
            return true;
        }

        if (!request.Cookies.ContainsKey(Cookies.Identity)
            && request.Headers.ContainsKey(Headers.Principal))
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var principalId = string.Empty;
                var principalName = string.Empty;

                if (request.Headers.ContainsKey(Headers.PrincipalId))
                {
                    principalId = request.Headers[Headers.PrincipalId];
                }
                if (request.Headers.ContainsKey(Headers.PrincipalName))
                {
                    principalName = request.Headers[Headers.PrincipalName];
                }

                if (string.IsNullOrEmpty(principalId))
                {
                    principalId = "[NotSet]";
                }
                if (string.IsNullOrEmpty(principalName))
                {
                    principalName = "[NotSet]";
                }

                _logger.LogInformation("Resolving identity details for {PrincipalId} and {TenantId}", principalId, tenantId);

                client.DefaultRequestHeaders.Add(Headers.Principal, request.Headers[Headers.Principal].ToString());
                client.DefaultRequestHeaders.Add(Headers.PrincipalId, principalId);
                client.DefaultRequestHeaders.Add(Headers.PrincipalName, principalName);
                client.DefaultRequestHeaders.Add(Headers.TenantId, tenantId.ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));

                var responseMessage = await client.GetAsync(_config.IdentityDetailsUrl);
                if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
                {
                    response.StatusCode = 403;
                    return false;
                }
                var identityDetails = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Error trying to resolve identity details for {PrincipalId} on tenant {TenantId}: {StatusCode} {ReasonPhrase}", principalId, tenantId, responseMessage.StatusCode, responseMessage.ReasonPhrase);
                    return true;
                }

                var encoding = Encoding.GetEncoding("iso-8859-1");
                var encoded = encoding.GetBytes(identityDetails);
                var identityDetailsAsBase64 = Convert.ToBase64String(encoded);
                response.Cookies.Append(Cookies.Identity, identityDetailsAsBase64, new CookieOptions { Expires = DateTimeOffset.MinValue });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to resolve identity details");
            }
        }

        return true;
    }
}

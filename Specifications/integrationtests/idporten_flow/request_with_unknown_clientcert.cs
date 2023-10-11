// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;
using Aksio.IngressMiddleware.integrationtests.idporten_flow.given;

namespace Aksio.IngressMiddleware.integrationtests.idporten_flow;

/// <summary>
/// Test scenario with Idporten flow.
/// </summary>
public class request_with_unknown_clientcert : factory_with_idporten
{
    HttpResponseMessage _responseMessage;
    readonly List<string> _redirectLog = new();

    async Task Because()
    {
        var args = new Dictionary<string, string>()
        {
            { "response_type", "code" },
            { "client_id", Guid.NewGuid().ToString() },
            { "redirect_uri", "http://host2/login/members/callback" },
            { "nonce", "qwertyuiop" },
            { "state", "redir=%2F" },
            { "scope", "openid+profile" }
        };
        using var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://auth.hostname/id-porten/authorize?{string.Join("&", args.Select(kvp => $"{kvp.Key}={kvp.Value}"))}");

        // Denne følger innover ser det ut til?!
        _responseMessage = await IngressClient.SendAsync(requestMessage);
        while (_responseMessage.StatusCode == HttpStatusCode.Redirect)
        {
            var newUri = _responseMessage.Headers.Location!;
            _redirectLog.Add(newUri.ToString());

            using var newRequest = new HttpRequestMessage(HttpMethod.Get, newUri);
            _responseMessage = await IngressClient.SendAsync(newRequest);
        }
    }

    [Fact]
    void was_redirected_through_idporten_authorization_endpoint() =>
        _redirectLog.ShouldContain(a => a.StartsWith(IngressConfig.IdPorten.AuthorizationEndpoint));
}
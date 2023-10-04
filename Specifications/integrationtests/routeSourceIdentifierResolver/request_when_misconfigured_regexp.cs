// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;
using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.integrationtests.routeSourceIdentifierResolver;

public class request_when_misconfigured_regexp : Specification
{
    HttpClient _ingressClient;
    HttpResponseMessage _responseMessage;

    void Establish()
    {
        var ingressConfig = new Config()
        {
            TenantResolutions = new[]
            {
                new TenantResolutionConfig()
                {
                    Strategy = TenantSourceIdentifierResolverType.Route,
                    Options = JsonSerializer.Deserialize<JsonObject>(
                        JsonSerializer.Serialize(new RouteSourceIdentifierOptions() { RegularExpression = ".*" }))
                }
            }
        };

        var ingressFactory = new IngressWebApplicationFactory
        {
            Config = ingressConfig
        };

        _ingressClient = ingressFactory.CreateClient();
    }

    async Task Because()
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/");
        
        requestMessage.Headers.Add(Headers.OriginalUri, "/2345/somethingelse");

        _responseMessage = await _ingressClient.SendAsync(requestMessage);
    }

    [Fact]
    void access_denied() => _responseMessage.StatusCode.Equals(StatusCodes.Status401Unauthorized);
}
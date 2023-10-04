// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.integrationtests.routeSourceIdentifierResolver.given;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

namespace Aksio.IngressMiddleware.integrationtests.routeSourceIdentifierResolver;

public class request_when_using_regexp : route_source_specification
{
    HttpClient _ingressClient;
    HttpResponseMessage _responseMessage;
    Guid _expectedTenantId;
    string _expectedSourceIdentifier;

    void Establish()
    {
        _expectedTenantId = Guid.NewGuid();
        _expectedSourceIdentifier = "1122";

        var ingressConfig = new Config()
        {
            Tenants = new()
            {
                { Guid.NewGuid(), new() { SourceIdentifiers = new[] { "8844" } } },
                { _expectedTenantId, new() { SourceIdentifiers = new[] { _expectedSourceIdentifier } } },
                { Guid.NewGuid(), new() { SourceIdentifiers = new[] { "9988" } } }
            },
            TenantResolutions = new[]
            {
                new TenantResolutionConfig()
                {
                    Strategy = TenantSourceIdentifierResolverType.Route,
                    Options = JsonSerializer.Deserialize<JsonObject>(
                        JsonSerializer.Serialize(
                            new RouteSourceIdentifierOptions() { RegularExpression = "^/(?<sourceIdentifier>[\\d]{4})/" }))
                }
            },
            Authorization = new()
            {
                {
                    "audienceWithNoAuth", new() { NoAuthorizationRequired = true }
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
        BuildAndSetPrincipalWithTenantClaim(requestMessage, "audienceWithNoAuth");

        requestMessage.Headers.Add(Headers.OriginalUri, $"/{_expectedSourceIdentifier}/blahblah");

        _responseMessage = await _ingressClient.SendAsync(requestMessage);

    }

    [Fact]
    void access_granted() => _responseMessage.IsSuccessStatusCode.ShouldBeTrue();

    [Fact]
    void got_the_expected_tenant() =>
        _responseMessage.Headers.GetValues("Tenant-ID").FirstOrDefault().ShouldEqual(_expectedTenantId.ToString());
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.BearerTokens;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

namespace Aksio.IngressMiddleware.integrationtests.oauth_bearertoken.given;

public class factory_with_oauth_bearertoken_auth_with_route_tenancyresolution : Specification
{
    protected IngressWebApplicationFactory IngressFactory;
    protected HttpClient IngressClient;
    protected Config IngressConfig;

    /// <summary>
    /// Set to false, to have the token validation "fail".
    /// </summary>
    protected bool IsTokenValid = true;

    void Establish()
    {
        IngressConfig = new()
        {
            Tenants = new()
            {
                {
                    Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    new() { SourceIdentifiers = new[] { "1122", "2266" } }
                },
                {
                    Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    new() { SourceIdentifiers = new[] { "2345", "5454" } }
                }
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
            OAuthBearerTokens = new() { Authority = "dummy://url" }
        };

        IngressFactory = new()
        {
            Config = IngressConfig
        };

        // Set up the mock oauth validator, this is the code responsible to talk to the authority and validate the tokens.
        Mock<IOAuthBearerTokenValidator> tokenValidatorMock = new();
        JwtSecurityToken jwtToken = new("issuer", "audience", new[] { new Claim("claimType", "claimValue") });
        tokenValidatorMock.Setup(_ => _.ValidateToken(IsAny<string>(), out jwtToken)).Returns(() => IsTokenValid);
        tokenValidatorMock.Setup(_ => _.RefreshJwks()).ReturnsAsync(true);
        IngressFactory.TransientServicesToReplace.Add((typeof(IOAuthBearerTokenValidator), tokenValidatorMock.Object));

        IngressClient = IngressFactory.CreateClient();
    }

    /// <summary>
    /// Helper to set the original uri and a fake bearer token the request.
    /// </summary>
    /// <param name="requestMessage">The request.</param>
    /// <param name="originalUri">The original request uri to fake.</param>
    protected void SetOriginalUrlAndAuthHeaders(HttpRequestMessage requestMessage, string originalUri)
    {
        requestMessage.Headers.Add(Headers.OriginalUri, originalUri);
        requestMessage.Headers.Authorization = new("Bearer", "dummytoken");
    }
}
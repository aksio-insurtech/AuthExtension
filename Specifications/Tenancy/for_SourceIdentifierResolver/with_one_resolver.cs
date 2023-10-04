// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Tenancy.for_SourceIdentifierResolver.given;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;
using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.Tenancy.for_SourceIdentifierResolver;

public class with_one_resolver : a_sourceidentifier_resolver
{
    Config _config;
    DefaultHttpContext _httpContext;
    string _expectedSourceIdentifier = "1234";
    string? _result;

    void Establish()
    {
        _config = new Config()
        {
            TenantResolutions = new[]
            {
                // First it should try regexp route matching.
                new TenantResolutionConfig()
                {
                    Strategy = TenantSourceIdentifierResolverType.Route,
                    Options = JsonSerializer.Deserialize<JsonObject>(
                        JsonSerializer.Serialize(
                            new RouteSourceIdentifierOptions() { RegularExpression = "^/(?<sourceIdentifier>[\\d]{4})/" }))
                },

                // And then it should fall back to principal claim matching.
                new TenantResolutionConfig()
                {
                    Strategy = TenantSourceIdentifierResolverType.Claim
                }
            }
        };

        _httpContext = GetHttpContext(null, $"/{_expectedSourceIdentifier}/other/route");
    }

    void Because() => _result = _resolver.Resolve(_config, _httpContext.Request);

    [Fact]
    void should_resolve_to_the_source_identifier() => _result.ShouldEqual(_expectedSourceIdentifier);
}
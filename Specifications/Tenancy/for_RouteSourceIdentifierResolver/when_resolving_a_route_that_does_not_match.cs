// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.Tenancy.for_RouteSourceIdentifierResolver;

public class when_resolving_a_route_that_does_not_match : Specification
{
    RouteSourceIdentifier _resolver;
    JsonObject _options;
    HttpContext _context;
    string _result;

    void Establish()
    {
        _options = JsonSerializer.Deserialize<JsonObject>(
            JsonSerializer.Serialize(
                new RouteSourceIdentifierOptions()
                {
                    RegularExpression = @"^/(?<sourceIdentifier>[\d]{4})/"
                }));

        _context = new DefaultHttpContext();
        _context.Request.Headers[Headers.OriginalUri] = "/foo/bar";

        _resolver = new(Mock.Of<ILogger<RouteSourceIdentifier>>());
    }

    void Because() => _result = _resolver.Resolve(_options, _context.Request);

    [Fact]
    void should_not_resolve_the_source_identifier() => _result?.ShouldBeNull();
}
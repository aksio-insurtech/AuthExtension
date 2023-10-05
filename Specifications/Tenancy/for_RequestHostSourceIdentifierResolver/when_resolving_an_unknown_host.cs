// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.Tenancy.for_RequestHostSourceIdentifierResolver;

public class when_resolving_an_unknown_host : Specification
{
    RequestHostSourceIdentifier _resolver;
    JsonObject _options;
    HttpContext _context;
    string _result;
    bool _success;
    string _requestHost = "customer.site.app";
    string _mappedSourceIdentifier = "2233";

    void Establish()
    {
        _options = JsonSerializer.Deserialize<JsonObject>(
            JsonSerializer.Serialize(
                new RequestHostSourceIdentifierOptions()
                {
                    Hostnames = new Dictionary<string, string>()
                    {
                        { _requestHost, _mappedSourceIdentifier }
                    }
                }));

        _context = new DefaultHttpContext();
        _context.Request.Host = new("another.hostname");

        _resolver = new(Mock.Of<ILogger<RequestHostSourceIdentifier>>());
    }

    void Because() => _success = _resolver.TryResolve(_options, _context.Request, out _result);

    [Fact]
    void should_not_resolve() => _success.ShouldBeFalse();
}
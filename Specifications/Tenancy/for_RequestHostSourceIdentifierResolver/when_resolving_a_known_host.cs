using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.Tenancy.for_RequestHostSourceIdentifierResolver;

public class when_resolving_a_known_host : Specification
{
    RequestHostSourceIdentifier _resolver;
    JsonObject _options;
    HttpContext _context;
    string _result;
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
        _context.Request.Host = new(_requestHost);

        _resolver = new(Mock.Of<ILogger<RequestHostSourceIdentifier>>());
    }

    void Because() => _result = _resolver.Resolve(_options, _context.Request);

    [Fact]
    void should_get_expected_sourceidentifier() => _result.ShouldEqual(_mappedSourceIdentifier);
}
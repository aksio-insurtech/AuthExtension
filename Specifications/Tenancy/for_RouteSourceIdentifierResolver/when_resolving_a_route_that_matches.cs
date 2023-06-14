// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.Tenancy.for_RouteSourceIdentifierResolver;

public class when_resolving_a_route_that_matches : Specification
{
    RouteSourceIdentifierResolver resolver;
    RouteSourceIdentifierResolverOptions options;
    HttpContext context;
    string result;

    void Establish()
    {
        options = new RouteSourceIdentifierResolverOptions
        {
            RegularExpression = @"^/(?<sourceIdentifier>[\d]{4})/"
        };

        context = new DefaultHttpContext();
        context.Request.Headers[Headers.OriginalUri] = "/3610/bar";

        resolver = new RouteSourceIdentifierResolver(Mock.Of<ILogger<RouteSourceIdentifierResolver>>());
    }

    async Task Because() => result = await resolver.Resolve(new Config(), options, context.Request);

    [Fact] void should_resolve_the_source_identifier() => result.ShouldEqual("3610");
}

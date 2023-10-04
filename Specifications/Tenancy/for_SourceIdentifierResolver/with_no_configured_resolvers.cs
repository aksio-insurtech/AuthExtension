// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Tenancy.for_SourceIdentifierResolver.given;
using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.Tenancy.for_SourceIdentifierResolver;

public class with_an_unknown_resolver : a_sourceidentifier_resolver
{
    Config _config;
    DefaultHttpContext _httpContext;
    Exception? _exception;

    void Establish()
    {
        // Set up a config with a source identifier which is not injected into _resolver.
        _config = new()
        {
            TenantResolutions = new List<TenantResolutionConfig>()
                { new() { Strategy = TenantSourceIdentifierResolverType.Host } }
        };

        _httpContext = GetHttpContext(null, "/1234/other/route");
    }

    void Because() =>
        _exception = Catch.Exception<TenantResolutionStrategyNotConfigured>(
            () => _resolver.Resolve(_config, _httpContext.Request));

    [Fact]
    void threw_the_expected_exception() => _exception.ShouldNotBeNull();
}
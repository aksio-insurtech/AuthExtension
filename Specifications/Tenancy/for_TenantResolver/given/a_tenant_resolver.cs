// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.Tenancy.for_TenantResolver.given;

public class a_tenant_resolver : Specification
{
    protected TenantResolver Resolver;
    protected Mock<ISourceIdentifierResolver> SourceIdentifierResolver;

    protected Config Config;
    protected DefaultHttpContext Context;

    void Establish()
    {
        SourceIdentifierResolver = new();
        Config = new();

        Resolver = new(Config, SourceIdentifierResolver.Object, Mock.Of<ILogger<TenantResolver>>());

        Context = new();
    }
}
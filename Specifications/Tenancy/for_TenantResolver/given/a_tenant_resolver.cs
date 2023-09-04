// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.Tenancy.for_TenantResolver.given;

public class a_tenant_resolver : Specification
{
    protected TenantResolver resolver;
    protected Mock<ITenantSourceIdentifierResolver> source_identifier_resolver;

    protected Config config;
    protected DefaultHttpContext context;

    void Establish()
    {
        source_identifier_resolver = new();
        config = new();

        resolver = new(
            config,
            source_identifier_resolver.Object,
            Mock.Of<ILogger<TenantResolver>>());

        context = new();
    }
}

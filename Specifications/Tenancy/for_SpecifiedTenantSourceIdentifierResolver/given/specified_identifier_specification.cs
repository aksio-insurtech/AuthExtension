// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;
using MELT;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.Tenancy.for_SpecifiedTenantSourceIdentifierResolver.given;

public class specified_identifier_specification : Specification
{
    protected SpecifiedSourceIdentifier Resolver;
    protected ITestLoggerFactory LoggerFactory;
    
    void Establish()
    {
        // https://alessio.franceschelli.me/posts/dotnet/how-to-test-logging-when-using-microsoft-extensions-logging/
        LoggerFactory = TestLoggerFactory.Create();
        var logger = LoggerFactory.CreateLogger<SpecifiedSourceIdentifier>();

        Resolver = new(logger);
    }
}
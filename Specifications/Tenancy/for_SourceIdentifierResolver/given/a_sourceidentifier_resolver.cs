// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;
using MELT;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.Tenancy.for_SourceIdentifierResolver.given;

public class a_sourceidentifier_resolver : Specification
{
    protected SourceIdentifierResolver _resolver;

    protected ITestLoggerFactory LoggerFactory;
    protected ILogger<SourceIdentifierResolver> Logger;

    protected RouteSourceIdentifier RouteResolver;
    protected RouteSourceIdentifierOptions RouteResolverOptions;

    protected ClaimsSourceIdentifier ClaimResolver;

    void Establish()
    {
        // https://alessio.franceschelli.me/posts/dotnet/how-to-test-logging-when-using-microsoft-extensions-logging/
        LoggerFactory = TestLoggerFactory.Create();
        Logger = LoggerFactory.CreateLogger<SourceIdentifierResolver>();

        RouteResolver = new(LoggerFactory.CreateLogger<RouteSourceIdentifier>());
        RouteResolverOptions = new() { RegularExpression = @"^/(?<sourceIdentifier>[\d]{4})/" };

        ClaimResolver = new(LoggerFactory.CreateLogger<ClaimsSourceIdentifier>());

        _resolver = new(new List<ISourceIdentifier> { RouteResolver, ClaimResolver }, Logger);
    }

    protected DefaultHttpContext GetHttpContext(string? claimedTenantId, string? requestUri)
    {
        DefaultHttpContext httpContext = new();
        if (!string.IsNullOrWhiteSpace(requestUri))
        {
            httpContext.Request.Headers[Headers.OriginalUri] = requestUri!;
        }

        var claims = new List<RawClaim>();
        if (!string.IsNullOrWhiteSpace(claimedTenantId))
        {
            claims.Add(new(ClaimsSourceIdentifier.EntraIdTenantIdClaim, claimedTenantId));
        }

        var principal = new RawClientPrincipal("testprovider", "testuser", "userdetails", claims);
        var jsonPrincipal = JsonSerializer.Serialize(principal, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        httpContext.Request.Headers.Add(Headers.Principal, Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonPrincipal)));

        return httpContext;
    }
}
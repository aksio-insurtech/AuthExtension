// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Tenancy.for_SpecifiedTenantSourceIdentifierResolver.given;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

namespace Aksio.IngressMiddleware.Tenancy.for_SpecifiedTenantSourceIdentifierResolver;

public class when_resolving_directly : specified_identifier_specification
{
    JsonObject _options;
    string _expectedSourceIdentifier = "someTenantIdentifier";
    string _resolvedTenant;

    void Establish()
    {
        _options = JsonSerializer.Deserialize<JsonObject>(
            JsonSerializer.Serialize(new SpecifiedSourceIdentifierOptions() { SourceIdentifier = _expectedSourceIdentifier }));
    }

    void Because() => _resolvedTenant = Resolver.Resolve(_options, null!);

    [Fact]
    void returned_correct_tenant() => _resolvedTenant.ShouldEqual(_expectedSourceIdentifier);
}
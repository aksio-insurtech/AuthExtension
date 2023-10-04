// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Tenancy.for_NoneSourceIdentifierResolver.given;

namespace Aksio.IngressMiddleware.Tenancy.for_NoneSourceIdentifierResolver;

public class when_resolving_directly : none_identifier_specification
{
    string _resolvedTenant;

    void Because() => _resolvedTenant = Resolver.Resolve(null!, null!);

    [Fact]
    void returns_empty_string() => _resolvedTenant.ShouldEqual(string.Empty);
}
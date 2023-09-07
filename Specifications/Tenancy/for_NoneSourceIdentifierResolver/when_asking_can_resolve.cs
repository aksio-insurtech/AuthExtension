// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy.for_NoneSourceIdentifierResolver;

public class when_asking_can_resolve : Specification
{
    SpecifiedSourceIdentifierResolver resolver;
    bool result;

    void Establish() => resolver = new();

    async Task Because() => result = await resolver.CanResolve(null!, null!, null!);

    [Fact]
    void should_be_able_to_resolve() => result.ShouldBeTrue();
}
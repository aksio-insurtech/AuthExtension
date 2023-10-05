// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;

namespace Aksio.IngressMiddleware.Tenancy.for_TenantResolver;

public class and_resolver_resolves_it : given.a_tenant_resolver
{
    Guid _tenantId = Guid.Parse("c392e7be-5cb4-4d1b-a461-7077e197309c");
    string _sourceIdentifier;
    TenantId _result;
    bool _success;

    void Establish()
    {
        _sourceIdentifier = "3610";
        Config.Tenants[_tenantId] = new()
        {
            SourceIdentifiers = new[] { _sourceIdentifier }
        };

        SourceIdentifierResolver.Setup(_ => _.TryResolve(Config, Context.Request, out _sourceIdentifier)).Returns(true);
    }

    void Because() => _success = Resolver.TryResolve(Context.Request, out _result);

    [Fact]
    void should_report_success() => _success.ShouldBeTrue();

    [Fact]
    void should_resolve_to_the_tenant_id() => _result.Value.ShouldEqual(_tenantId);
}
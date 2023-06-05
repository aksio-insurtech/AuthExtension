// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Tenancy;

namespace Aksio.IngressMiddleware.Impersonation;

public class TenantImpersonationAuthorizer : IImpersonationAuthorizer
{
    readonly Config _config;
    readonly ITenantResolver _tenantResolver;

    public TenantImpersonationAuthorizer(
        Config config,
        ITenantResolver tenantResolver)
    {
        _config = config;
        _tenantResolver = tenantResolver;
    }

    public async Task<bool> IsAuthorized(HttpRequest request, ClientPrincipal principal)
    {
        if (!_config.Impersonation.Authorization.Tenants.Any())
        {
            return true;
        }

        var tenantId = await _tenantResolver.Resolve(request);
        return _config.Impersonation.Authorization.Tenants.Any(_ => _.Equals(tenantId.ToString(), StringComparison.InvariantCultureIgnoreCase));
    }
}

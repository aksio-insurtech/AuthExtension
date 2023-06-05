// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation;

public class TenantImpersonationAuthorizer : IImpersonationAuthorizer
{
    public TenantImpersonationAuthorizer(Config config)
    {
    }

    public Task<bool> IsAuthorized(HttpRequest request, ClientPrincipal principal)
    {
        throw new NotImplementedException();
    }
}

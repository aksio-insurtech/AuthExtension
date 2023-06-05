// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Impersonation;

public class ClaimImpersonationAuthorizer : IImpersonationAuthorizer
{
    public Task<bool> IsAuthorized(HttpRequest request, ClientPrincipal principal)
    {
        throw new NotImplementedException();
    }
}
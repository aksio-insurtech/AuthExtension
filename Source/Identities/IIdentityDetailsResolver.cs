// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;

namespace Aksio.IngressMiddleware.Identities;

public interface IIdentityDetailsResolver
{
    Task<bool> Resolve(HttpRequest request, HttpResponse response, TenantId tenantId);
}

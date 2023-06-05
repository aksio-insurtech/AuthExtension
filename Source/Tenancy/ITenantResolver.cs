// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;

namespace Aksio.IngressMiddleware.Tenancy;

public interface ITenantResolver
{
    Task<TenantId> Resolve(HttpRequest request);
}

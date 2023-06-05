// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;

namespace Aksio.IngressMiddleware.BearerTokens;

public interface IOAuthBearerTokens
{
    Task<IActionResult> Handle(HttpRequest request, HttpResponse response, TenantId tenantId);
}

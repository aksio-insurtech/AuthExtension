// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;

namespace Aksio.IngressMiddleware.BearerTokens;

/// <summary>
/// Defines a system that can handle OAuth bearer tokens.
/// </summary>
public interface IOAuthBearerTokens
{
    /// <summary>
    /// Handle the OAuth bearer token.
    /// </summary>
    /// <param name="request"><see cref="HttpRequest"/>.</param>
    /// <param name="response"><see cref="HttpResponse"/>.</param>
    /// <param name="tenantId"><see cref="TenantId"/>.</param>
    /// <returns><see cref="IActionResult"/>.</returns>
    Task<IActionResult> Handle(HttpRequest request, HttpResponse response, TenantId tenantId);
}

// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.RoleAuthorization;

/// <summary>
/// Defines a system that can handle mTLS/Mutual TLS, a.k.a. client certificate requests.
/// Defines roles that are needed to accept an (e.g. entraId/AAD) login.
/// </summary>
public interface IRoleAuthorizer
{
    /// <summary>
    /// Handle the request.
    /// </summary>
    /// <param name="request"><see cref="HttpRequest"/>.</param>
    /// <returns>OkResult() on success, or StatusCodeResult(StatusCodes.Status403Forbidden) if not authenticated.</returns>
    IActionResult Handle(HttpRequest request);
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.EntraIdRoles;

/// <summary>
/// Defines a system that can handle mTLS/Mutual TLS, a.k.a. client certificate requests.
/// Defines roles that are needed to accept an entraId/AAD login.
/// </summary>
public interface IEntraIdRoles
{
    // /// <summary>
    // /// Checks if entra id roles are configured, and thus should be called.
    // /// </summary>
    // /// <returns>True if configured.</returns>
    // bool IsEnabled();

    /// <summary>
    /// Handle the request.
    /// </summary>
    /// <param name="request"><see cref="HttpRequest"/>.</param>
    /// <returns>OkResult() on success, or StatusCodeResult(StatusCodes.Status403Forbidden) if not authenticated.</returns>
    IActionResult Handle(HttpRequest request);
}
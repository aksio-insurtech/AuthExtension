// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Impersonation;

/// <summary>
/// Represents the impersonation flow for regular requests.
/// </summary>
public interface IImpersonationFlow
{
    /// <summary>
    /// Handles impersonation for the request.
    /// </summary>
    /// <param name="request"><see cref="HttpRequest"/> instance.</param>
    /// <param name="response"><see cref="HttpResponse"/> instance.</param>
    /// <returns>True if request is impersonated, false if not.</returns>
    bool HandleImpersonatedPrincipal(HttpRequest request, HttpResponse response);

    /// <summary>
    /// Checks if the request should be impersonated.
    /// </summary>
    /// <param name="request"><see cref="HttpRequest"/> instance.</param>
    /// <returns>True if it should be impersonated, false if not.</returns>
    bool ShouldImpersonate(HttpRequest request);

    /// <summary>
    /// Checks if the request is an impersonation route.
    /// </summary>
    /// <param name="request"><see cref="HttpRequest"/> instance.</param>
    /// <returns>True if it is the impersonation route.</returns>
    bool IsImpersonateRoute(HttpRequest request);
}

// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Impersonation;

/// <summary>
/// Defines an impersonation authorizer.
/// </summary>
public interface IImpersonationAuthorizer
{
    /// <summary>
    /// Checks if the request is authorized for impersonation.
    /// </summary>
    /// <param name="request"><see cref="HttpRequest"/> to check for.</param>
    /// <param name="principal">The current <see cref="ClientPrincipal"/>.</param>
    /// <returns>True if authorized, false if not.</returns>
    Task<bool> IsAuthorized(HttpRequest request, ClientPrincipal principal);
}

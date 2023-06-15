// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;

namespace Aksio.IngressMiddleware.Identities;

/// <summary>
/// Defines a system that can resolve the details of an identity.
/// </summary>
public interface IIdentityDetailsResolver
{
    /// <summary>
    /// Resolve the details of an identity.
    /// </summary>
    /// <param name="request"><see cref="HttpRequest"/> to resolve from.</param>
    /// <param name="response"><see cref="HttpResponse"/> to provide feedback on.</param>
    /// <param name="tenantId"><see cref="TenantId"/> to resolve for.</param>
    /// <param name="isImpersonated">Whether or not this is impersonation, which means we ignore current identity. Defaults to false.</param>
    /// <returns>True if could resolve, false if not.</returns>
    Task<bool> Resolve(HttpRequest request, HttpResponse response, TenantId tenantId, bool isImpersonated = false);
}

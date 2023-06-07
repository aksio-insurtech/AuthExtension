// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the configuration related to impersonation.
/// </summary>
public class ImpersonationConfig
{
    /// <summary>
    /// Gets or sets the collection of identity providers that are allowed to impersonate. If none are specified, impersonation is not allowed.
    /// </summary>
    public IEnumerable<string> IdentityProviders { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// Gets or sets the <see cref="ImpersonationAuthorizationConfig"/> configuration.
    /// </summary>
    public ImpersonationAuthorizationConfig Authorization { get; set; } = new ImpersonationAuthorizationConfig();
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the configuration related to authorization of impersonation.
/// </summary>
public class ImpersonationAuthorizationConfig
{
    /// <summary>
    /// Gets or sets allowed collection of tenants. If none are specified, all tenants are allowed.
    /// </summary>
    public IEnumerable<string> Tenants { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// Gets or sets allowed collection of roles. If none are specified, all roles are allowed.
    /// </summary>
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// Gets or sets allowed collection of groups. If none are specified, all groups are allowed.
    /// </summary>
    public IEnumerable<string> Groups { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// Gets or sets allowed collection of claims. If none are specified, all claims are allowed.
    /// </summary>
    public IEnumerable<Claim> Claims { get; set; } = Enumerable.Empty<Claim>();
}

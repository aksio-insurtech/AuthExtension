// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the configuration of a tenant.
/// </summary>
public record TenantConfig
{
    /// <summary>
    /// Gets or sets the domain name that represents the tenant.
    /// </summary>
    public string Domain { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the behalf of string.
    /// </summary>
    public string OnBehalfOf { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a collection of source identifiers of tenants that are allowed to impersonate this tenant.
    /// </summary>
    /// <remarks>
    /// If the tenant specified is the actual tenant, it is not impersonating that tenant.
    /// </remarks>
    public string[] SourceIdentifiers { get; set; } = Array.Empty<string>();
}

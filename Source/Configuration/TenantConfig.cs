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
    /// Gets or sets a collection of source identifiers used to look up the tenant.
    /// </summary>
    public string[] SourceIdentifiers { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Gets or sets a collection of entra id tenant identifiers allowed to log in with this middleware tenant.
    /// </summary>
    public string[] EntraIdTenants { get; set; } = Array.Empty<string>();
}
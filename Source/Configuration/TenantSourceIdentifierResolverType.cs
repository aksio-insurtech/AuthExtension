// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the configuration for a tenant.
/// </summary>
public enum TenantSourceIdentifierResolverType
{
    /// <summary>
    /// No strategy is used.
    /// </summary>
    None = 0,

    /// <summary>
    /// The tenant identifier is resolved from the route.
    /// </summary>
    Route = 1,

    /// <summary>
    /// The tenant identifier is resolved from a claim.
    /// </summary>
    Claim = 2,

    /// <summary>
    /// The tenant identifier is resolved to a specific single tenant.
    /// </summary>
    Specified = 3
}

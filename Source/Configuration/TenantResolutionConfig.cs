// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Nodes;

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the configuration related to tenant resolution.
/// </summary>
public class TenantResolutionConfig
{
    /// <summary>
    /// Gets or sets the strategy to use for resolving the tenant identifier.
    /// </summary>
    public TenantSourceIdentifierResolverType Strategy { get; set; } = TenantSourceIdentifierResolverType.Undefined;

    /// <summary>
    /// Gets or sets the options for the strategy.
    /// </summary>
    public JsonObject Options { get; set; } = new JsonObject();
}

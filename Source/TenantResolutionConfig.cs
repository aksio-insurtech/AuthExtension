// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Nodes;

namespace Aksio.IngressMiddleware;

public class TenantResolutionConfig
{
    public TenantSourceIdentifierResolverType Strategy { get; set; } = TenantSourceIdentifierResolverType.None;

    public JsonObject Options { get; set; } = new JsonObject();
}

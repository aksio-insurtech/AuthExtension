// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware;

public record TenantConfig
{
    public string Domain { get; set; } = "localhost";
    public string OnBehalfOf { get; set; } = string.Empty;
    public string[] TenantIdClaims { get; set; } = Array.Empty<string>();
}

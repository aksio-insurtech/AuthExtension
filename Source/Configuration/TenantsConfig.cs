// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the configuration for all tenants.
/// </summary>
public class TenantsConfig : Dictionary<Guid, TenantConfig>
{
}

// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Exception that gets thrown when tenant resolution strategy is not defined in the configuration.
/// </summary>
public class TenantResolutionStrategyNotConfigured : Exception
{
    /// <summary>
    /// TenantResolution strategy is not configured.
    /// </summary>
    public TenantResolutionStrategyNotConfigured()
        : base("TenantResolution strategy is not configured! It must be one of: 'None', 'Route', 'Claim', or 'Specified'")
    {
    }
}
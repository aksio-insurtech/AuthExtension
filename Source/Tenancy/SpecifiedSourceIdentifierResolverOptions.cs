// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Represents configuration options for <see cref="SpecifiedSourceIdentifierResolver"/>.
/// </summary>
public class SpecifiedSourceIdentifierResolverOptions
{
    /// <summary>
    /// The specific tenant to resolve to.
    /// </summary>
    public string TenantId { get; set; } = Guid.Empty.ToString();
}

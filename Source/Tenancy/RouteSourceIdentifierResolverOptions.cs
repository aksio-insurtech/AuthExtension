// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Represents configuration options for <see cref="RouteSourceIdentifierResolver"/>.
/// </summary>
public class RouteSourceIdentifierResolverOptions
{
    /// <summary>
    /// Gets or sets the regular expression to use for extracting the source identifier.
    /// </summary>
    public string RegularExpression { get; set; } = string.Empty;
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

/// <summary>
/// Represents configuration options for <see cref="RouteSourceIdentifier"/>.
/// </summary>
public class RouteSourceIdentifierOptions
{
    /// <summary>
    /// Gets or sets the regular expression to use for extracting the source identifier.
    /// </summary>
    public string RegularExpression { get; set; } = string.Empty;
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

/// <summary>
/// Represents configuration options for <see cref="SpecifiedSourceIdentifier"/>.
/// </summary>
public class SpecifiedSourceIdentifierOptions
{
    /// <summary>
    /// The specific tenant to resolve to.
    /// </summary>
    public string SourceIdentifier { get; set; } = string.Empty;
}

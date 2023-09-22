// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the configuration for entra id roles requirement.
/// </summary>
public class EntraIdRolesConfig
{
    /// <summary>
    /// Gets or sets the list of accepted role values.
    /// </summary>
    public IList<string> AcceptedRoles { get; set; } = new List<string>();
}
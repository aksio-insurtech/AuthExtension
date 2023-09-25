// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the configuration for entra id roles requirement.
/// </summary>
public class RoleAuthorizationConfig
{
    /// <summary>
    /// Can be set to true to set up an ingress that does not require a role (only Entra ID login).
    /// </summary>
    public bool NoRoleRequired { get; set; }

    /// <summary>
    /// The list of accepted role values.
    /// </summary>
    public IEnumerable<string> AcceptedRoles { get; set; } = Enumerable.Empty<string>();
}
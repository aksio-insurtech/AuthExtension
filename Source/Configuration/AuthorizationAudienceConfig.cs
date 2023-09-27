// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the configuration for authorization requirement, such as roles.
/// </summary>
public class AuthorizationAudienceConfig
{
    /// <summary>
    /// Can be set to true to set up an ingress that does not require a role (only Entra ID login).
    /// </summary>
    public bool NoAuthorizationRequired { get; set; }

    /// <summary>
    /// The list of accepted role values.
    /// </summary>
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the configuration for an OpenID Connect identity provider.
/// </summary>
public class OpenIDConnectConfig
{
    /// <summary>
    /// Gets or sets the issuer URL. Typically the well known configuration would be found at this URL + /.well-known/openid-configuration.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the actual authorization endpoint for the identity provider.
    /// </summary>
    public string AuthorizationEndpoint { get; set; } = string.Empty;
}

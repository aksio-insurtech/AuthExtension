// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the configuration for OAuth bearer tokens.
/// </summary>
public class OAuthBearerTokensConfig
{
    /// <summary>
    /// Gets whether or not the configuration is enabled.
    /// </summary>
    public bool IsEnabled => !string.IsNullOrEmpty(Authority);

    /// <summary>
    /// Gets or sets the well known URL for document that describes the authority.
    /// </summary>
    public string Authority { get; set; } = string.Empty;
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the root configuration object for the ingress middleware.
/// </summary>
public class Config
{
    /// <summary>
    /// Gets or sets the <see cref="OpenIDConnectConfig"/> configuration related to IdPorten.
    /// </summary>
    public OpenIDConnectConfig IdPorten { get; set; } = new();

    /// <summary>
    /// Gets or sets the <see cref="TenantsConfig"/>.
    /// </summary>
    public TenantsConfig Tenants { get; set; } = new();

    /// <summary>
    /// Gets or sets the <see cref="TenantResolution"/> configuration.
    /// </summary>
    public TenantResolutionConfig TenantResolution { get; set; } = new();

    /// <summary>
    /// Gets or sets the URL to use for getting identity details.
    /// </summary>
    public string IdentityDetailsUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the <see cref="OAuthBearerTokensConfig"/> configuration.
    /// </summary>
    public OAuthBearerTokensConfig OAuthBearerTokens { get; set; } = new();

    /// <summary>
    /// Gets or sets the <see cref="ImpersonationConfig"/> configuration.
    /// </summary>
    public ImpersonationConfig Impersonation { get; set; } = new();

    /// <summary>
    /// Gets or sets the <see cref="MutualTLSConfig"/> configuration.
    /// </summary>
    public MutualTLSConfig MutualTLS { get; set; } = new();

    /// <summary>
    /// Gets or sets the <see cref="RoleAuthorizationConfig"/> configuration.
    /// </summary>
    public RoleAuthorizationConfig RoleAuthorization { get; set; } = new();

    /// <summary>
    /// Loads the configuration from the file system.
    /// </summary>
    /// <returns>A new <see cref="Config"/> instance.</returns>
    public static Config Load()
    {
        const string configFile = "./config/config.json";
        var config = new Config();
        if (File.Exists(configFile))
        {
            var configJson = File.ReadAllText(configFile);
            config = JsonSerializer.Deserialize<Config>(configJson, Globals.JsonSerializerOptions)!;
        }

        return config;
    }
}

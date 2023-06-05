// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace Aksio.IngressMiddleware.Configuration;

public class Config
{
    public OpenIDConnectConfig IdPorten { get; set; } = new OpenIDConnectConfig();
    public TenantsConfig Tenants { get; set; } = new TenantsConfig();
    public TenantResolutionConfig TenantResolution { get; set; } = new TenantResolutionConfig();
    public string IdentityDetailsUrl { get; set; } = string.Empty;
    public OAuthBearerTokensConfig OAuthBearerTokens { get; set; } = new OAuthBearerTokensConfig();
    public ImpersonationConfig Impersonation { get; set; } = new ImpersonationConfig();

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

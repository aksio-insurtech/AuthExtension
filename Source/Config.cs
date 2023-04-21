// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Aksio.IngressMiddleware;

public class Config
{
    public OpenIDConnectConfig IdPorten { get; set; } = new OpenIDConnectConfig();
    public TenantsConfig Tenants { get; set; } = new TenantsConfig();
    public string IdentityDetailsUrl { get; set; } = string.Empty;
    public OAuthBearerTokensConfig OAuthBearerTokens { get; set; } = new OAuthBearerTokensConfig();
}

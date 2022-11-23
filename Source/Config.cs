// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Aksio.IngressMiddleware;

public class Config
{
    public AzureAdConfig AzureAd { get; set; } = new AzureAdConfig();
    public IdPortenConfig IdPorten { get; set; } = new IdPortenConfig();
    public TenantsConfig Tenants { get; set; } = new TenantsConfig();
}

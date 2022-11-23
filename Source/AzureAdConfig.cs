// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Aksio.IngressMiddleware;

public class AzureAdConfig
{
    public string Issuer { get; set; } = string.Empty;
    public string AuthorizeEndpoint { get; set; } = string.Empty;
}
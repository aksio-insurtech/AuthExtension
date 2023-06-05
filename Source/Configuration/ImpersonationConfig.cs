// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Configuration;

public class ImpersonationConfig
{
    public IEnumerable<string> IdentityProviders { get; set; } = Enumerable.Empty<string>();
    public ImpersonationAuthorizationConfig Authorization { get; set; } = new ImpersonationAuthorizationConfig();
}
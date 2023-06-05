// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware.Configuration;

public class ImpersonationAuthorizationConfig
{
    public IEnumerable<string> Tenants { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<string> Groups { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<Claim> Claims { get; set; } = Enumerable.Empty<Claim>();
}

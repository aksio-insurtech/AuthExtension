// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware;

public record ClientPrincipal(string IdentityProvider, string UserId, string UserDetails, IEnumerable<string> UserRoles, IEnumerable<Claim> Claims);

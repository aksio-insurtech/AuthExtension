// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware;

#pragma warning disable CA1707 // Identifiers should not contain underscores

public record AuthorityResult(string issuer, string jwks_uri);
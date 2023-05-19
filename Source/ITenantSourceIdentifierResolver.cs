// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware;


public interface ITenantSourceIdentifierResolver
{
    Task<string> Resolve(Config config, HttpRequest request);
}

public interface ITenantSourceIdentifierResolver<TOptions> : ITenantSourceIdentifierResolver
{
    Task<string> Resolve(Config config, TOptions options, HttpRequest request);
}

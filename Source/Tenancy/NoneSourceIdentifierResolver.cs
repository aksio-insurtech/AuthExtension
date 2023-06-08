// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="ITenantResolver"/> that always resolves the tenant to NotSet.
/// </summary>
public class NoneSourceIdentifierResolver : ITenantSourceIdentifierResolver
{
    /// <inheritdoc/>
    public Task<TenantId> Resolve(Config config, HttpRequest request) => Task.FromResult(TenantId.NotSet);
}

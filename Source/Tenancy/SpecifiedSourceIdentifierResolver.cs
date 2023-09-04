// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="ITenantSourceIdentifierResolver"/> that always resolves the tenant to a specific tenant.
/// </summary>
public class SpecifiedSourceIdentifierResolver : TenantSourceIdentifierResolver, ITenantSourceIdentifierResolver<SpecifiedSourceIdentifierResolverOptions>
{
    /// <inheritdoc/>
    public Task<bool> CanResolve(Config config, SpecifiedSourceIdentifierResolverOptions options, HttpRequest request) => Task.FromResult(true);

    /// <inheritdoc/>
    public Task<string> Resolve(Config config, SpecifiedSourceIdentifierResolverOptions options, HttpRequest request) => Task.FromResult(options.TenantId);
}

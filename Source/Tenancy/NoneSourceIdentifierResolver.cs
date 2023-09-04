// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="ITenantResolver"/> that always resolves the tenant to NotSet.
/// </summary>
public class NoneSourceIdentifierResolver : ITenantSourceIdentifierResolver
{
    /// <inheritdoc/>
    public Task<bool> CanResolve(Config config, HttpRequest request) => Task.FromResult(true);

    /// <inheritdoc/>
    public Task<string> Resolve(Config config, HttpRequest request) => Task.FromResult(string.Empty);
}

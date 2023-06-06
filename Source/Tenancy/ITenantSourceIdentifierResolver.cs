// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Defines a tenant source identifier resolver.
/// </summary>
public interface ITenantSourceIdentifierResolver
{
    /// <summary>
    /// Resolve the tenant from the given <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="request"><see cref="HttpRequest"/> to resolve from.</param>
    /// <returns></returns>
    Task<TenantId> Resolve(Config config, HttpRequest request);
}

/// <summary>
/// Defines a tenant source identifier resolver for a specific options configuration.
/// </summary>
/// <typeparam name="TOptions">Type of options.</typeparam>
public interface ITenantSourceIdentifierResolver<TOptions> : ITenantSourceIdentifierResolver
{
    Task<TenantId> Resolve(Config config, TOptions options, HttpRequest request);
}

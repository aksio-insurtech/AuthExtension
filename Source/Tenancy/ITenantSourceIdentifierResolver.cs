// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Defines a tenant source identifier resolver.
/// </summary>
public interface ITenantSourceIdentifierResolver
{
    /// <summary>
    /// Check if the resolver can resolve the tenant from the given <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="request"><see cref="HttpRequest"/> to resolve from.</param>
    /// <returns>True if it can, false if not.</returns>
    Task<bool> CanResolve(Config config, HttpRequest request);

    /// <summary>
    /// Resolve the tenant from the given <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="request"><see cref="HttpRequest"/> to resolve from.</param>
    /// <returns>Resolved source identifier.</returns>
    Task<string> Resolve(Config config, HttpRequest request);
}

/// <summary>
/// Defines a tenant source identifier resolver for a specific options configuration.
/// </summary>
/// <typeparam name="TOptions">Type of options.</typeparam>
public interface ITenantSourceIdentifierResolver<TOptions> : ITenantSourceIdentifierResolver
{
    /// <summary>
    /// Check if the resolver can resolve the tenant from the given <see cref="HttpRequest"/>. 
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="options">The options associated with the resolver.</param>
    /// <param name="request"><see cref="HttpRequest"/> to resolve from.</param>
    /// <returns>True if it can, false if not.</returns>
    Task<bool> CanResolve(Config config, TOptions options, HttpRequest request);

    /// <summary>
    /// Resolve the tenant from the given <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="options">The options associated with the resolver.</param>
    /// <param name="request"><see cref="HttpRequest"/> to resolve from.</param>
    /// <returns>Resolved source identifier.</returns>
    Task<string> Resolve(Config config, TOptions options, HttpRequest request);
}

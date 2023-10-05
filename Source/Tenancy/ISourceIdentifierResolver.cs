// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Defines the tenant source identifier resolver manager.
/// </summary>
public interface ISourceIdentifierResolver
{
    /// <summary>
    /// Resolve the tenant from the given <see cref="HttpRequest"/>.
    /// Will call the configured resolver implementations.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="request"><see cref="HttpRequest"/> to resolve from.</param>
    /// <param name="sourceIdentifier">The resolved source identifier.</param>
    /// <returns>True if successful, false if unable to resolve.</returns>
    bool TryResolve(Config config, HttpRequest request, out string sourceIdentifier);
}
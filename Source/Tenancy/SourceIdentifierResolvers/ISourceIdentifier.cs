// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Nodes;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

/// <summary>
/// Defines a the implemenatation of a tenant source identifier resolver.
/// </summary>
public interface ISourceIdentifier
{
    /// <summary>
    /// Which resolver type this implements.
    /// </summary>
    TenantSourceIdentifierResolverType ResolverType { get; }

    /// <summary>
    /// Resolve the source identifier from the given <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="options">The instance config.</param>
    /// <param name="request"><see cref="HttpRequest"/> to resolve from.</param>
    /// <param name="sourceIdentifier">The resolved source identifier.</param>
    /// <returns>True if successful, false if unable to resolve.</returns>
    bool TryResolve(JsonObject options, HttpRequest request, out string sourceIdentifier);
}
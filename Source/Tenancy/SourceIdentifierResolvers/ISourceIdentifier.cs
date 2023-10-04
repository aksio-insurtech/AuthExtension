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
    /// <returns>Resolved source identifier (string.Empty instructs caller to use TenantId.NotSet), or null if unable to resolve.</returns>
    string? Resolve(JsonObject options, HttpRequest request);
}
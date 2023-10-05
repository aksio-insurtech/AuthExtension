// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;

/// <summary>
/// Represents configuration options for <see cref="RequestHostSourceIdentifier"/>.
/// </summary>
public class RequestHostSourceIdentifierOptions
{
    /// <summary>
    /// The hostname(s) to validate against, and the source identifier to resolve to.
    /// This can e.g. be "aksio.customer.com", "2222" which then will result in identifier 2222 - which can be matched against the tenant configuration.
    /// </summary>
    public IDictionary<string, string> Hostnames { get; set; } = new Dictionary<string, string>();
}

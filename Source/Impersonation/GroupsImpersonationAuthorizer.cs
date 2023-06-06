// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation;

/// <summary>
/// Represents an authorizer for impersonation based on groups.
/// </summary>
public class GroupsImpersonationAuthorizer : IImpersonationAuthorizer
{
    readonly Config _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupsImpersonationAuthorizer"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    public GroupsImpersonationAuthorizer(Config config)
    {
        _config = config;
    }

    /// <inheritdoc/>
    public Task<bool> IsAuthorized(HttpRequest request, ClientPrincipal principal) => throw new NotImplementedException();
}
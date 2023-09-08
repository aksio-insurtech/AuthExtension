// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;
using Aksio.Execution;

namespace Aksio.IngressMiddleware.Identities;

static partial class IdentityDetailsResolverLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Identity details url is not configured, skipping identity details resolution")]
    internal static partial void IdentityDetailsUrlNotConfigured(this ILogger<IdentityDetailsResolver> logger);

    [LoggerMessage(1, LogLevel.Information, "Resolving identity details for {PrincipalId} and {TenantId}")]
    internal static partial void ResolvingIdentityDetails(this ILogger<IdentityDetailsResolver> logger, string principalId, TenantId tenantId);

    [LoggerMessage(2, LogLevel.Error, "Error trying to resolve identity details for {PrincipalId} on tenant {TenantId}: {StatusCode} {ReasonPhrase}")]
    internal static partial void ErrorResolvingIdentityDetails(this ILogger<IdentityDetailsResolver> logger, string principalId, TenantId tenantId, HttpStatusCode statusCode, string reasonPhrase);

    [LoggerMessage(3, LogLevel.Error, "Principal {PrincipalId} on tenant {TenantId} is forbidden from accessing the system: '{ReasonPhrase}'")]
    internal static partial void Forbidden(this ILogger<IdentityDetailsResolver> logger, string principalId, TenantId tenantId, string reasonPhrase);
}
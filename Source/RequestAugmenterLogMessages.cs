// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware;

static partial class RequestAugmenterLogMessages
{
    [LoggerMessage(0, LogLevel.Warning, "Unauthorized access rejected, no tenant id was resolved.")]
    internal static partial void UnauthorizedBecauseNoTenantIdWasResolved(this ILogger<RequestAugmenter> logger);

    [LoggerMessage(1, LogLevel.Information, "Accepting pre-approved uri {PreApprovedUri}, client address = {ClientIp}")]
    internal static partial void AcceptingPreApprovedUri(
        this ILogger<RequestAugmenter> logger,
        string preApprovedUri,
        string clientIp);
}
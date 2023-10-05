// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware;

static partial class RequestAugmenterLogMessages
{
    [LoggerMessage(0, LogLevel.Warning, "Unauthorized access rejected, no tenant id was resolved.")]
    internal static partial void UnauthorizedBecauseNoTenantIdWasResolved(this ILogger<RequestAugmenter> logger);
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware;

/// <summary>
/// Represents all the cookies used by the ingress middleware.
/// </summary>
public static class Cookies
{
    /// <summary>
    /// Name of the identity cookie.
    /// </summary>
    public const string Identity = ".aksio-identity";

    /// <summary>
    /// Name of the impersonation cookie.
    /// </summary>
    public const string Impersonation = ".aksio-identity-impersonation";
}
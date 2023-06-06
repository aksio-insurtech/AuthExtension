// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware;

/// <summary>
/// Represents all the HTTP headers used by the ingress middleware.
/// </summary>
public static class Headers
{
    /// <summary>
    /// Microsoft Identity Platform client principal.
    /// </summary>
    public const string Principal = "x-ms-client-principal";

    /// <summary>
    /// Microsoft Identity Platform client principal ID.
    /// </summary>
    public const string PrincipalId = "x-ms-client-principal-id";

    /// <summary>
    /// Microsoft Identity Platform client principal name.
    /// </summary>
    public const string PrincipalName = "x-ms-client-principal-name";

    /// <summary>
    /// Original URI.
    /// </summary>
    public const string OriginalUri = "x-original-uri";

    /// <summary>
    /// Aksio tenant identifier.
    /// </summary>
    public const string TenantId = "Tenant-ID";

    /// <summary>
    /// Authorization header.
    /// </summary>
    public const string Authorization = "Authorization";
}

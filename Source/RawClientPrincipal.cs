// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware;

/// <summary>
/// Represents a raw client principal based on the Microsoft Identity Platform.
/// </summary>
/// <param name="auth_typ">Auth type (identity provider).</param>
/// <param name="name_typ">Name claim.</param>
/// <param name="role_typ">Role claim.</param>
/// <param name="claims">All the claims.</param>
record RawClientPrincipal(string auth_typ, string name_typ, string role_typ, IEnumerable<RawClaim> claims);

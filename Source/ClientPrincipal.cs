// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json;
using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware;

/// <summary>
/// Represents a client principal based on the Microsoft Identity Platform.
/// </summary>
/// <param name="IdentityProvider">A string representing the identity provider, e.g. aad.</param>
/// <param name="Issuer">A string representing the token issuer.</param>
/// <param name="UserId">The unique user identifier from the identity provider.</param>
/// <param name="UserDetails">The user details, typically the user name.</param>
/// <param name="UserRoles">Collection of roles for the user.</param>
/// <param name="Claims">Collection of all the claims for the user.</param>
public record ClientPrincipal(string IdentityProvider, string Issuer, string UserId, string UserDetails, IEnumerable<string> UserRoles, IEnumerable<Claim> Claims)
{
    /// <summary>
    /// The name type.
    /// </summary>
    public string NameType { get; set; } = string.Empty;

    /// <summary>
    /// The role type.
    /// </summary>
    public string RoleType { get; set; } = string.Empty;

    /// <summary>
    /// Represents an empty <see cref="ClientPrincipal"/>.
    /// </summary>
    public static readonly ClientPrincipal Empty = new(string.Empty, string.Empty, string.Empty, string.Empty, Array.Empty<string>(), Array.Empty<Claim>());

    /// <summary>
    /// Convert from a base64 encoded string to a <see cref="ClientPrincipal"/>.
    /// </summary>
    /// <param name="userId">The user identifier to convert from.</param>
    /// <param name="base64">The base64 string representation.</param>
    /// <returns>A new <see cref="ClientPrincipal"/> instance.</returns>
    public static ClientPrincipal FromBase64(string userId, string base64)
    {
        var json = Convert.FromBase64String(base64);
        var jsonText = Encoding.UTF8.GetString(json);
        var rawPrincipal = JsonSerializer.Deserialize<RawClientPrincipal>(jsonText, Globals.JsonSerializerOptions)!;

        var name = string.IsNullOrEmpty(rawPrincipal.name_typ) ? string.Empty : rawPrincipal.claims.FirstOrDefault(_ => _.Type == rawPrincipal.name_typ)?.Value ?? string.Empty;
        var roles = string.IsNullOrEmpty(rawPrincipal.role_typ) ? Enumerable.Empty<string>() : rawPrincipal.claims.Where(_ => _.Type == rawPrincipal.role_typ).Select(_ => _.Value).ToArray();
        var claims = rawPrincipal.claims?.ToClaims()?.ToList() ?? new();
        var issuer = claims.SingleOrDefault(c => c.Type == "iss")?.Value ?? string.Empty;
        return new(rawPrincipal.auth_typ, issuer, userId ?? string.Empty, name, roles, claims)
        {
            NameType = rawPrincipal.name_typ,
            RoleType = rawPrincipal.role_typ
        };
    }

    /// <summary>
    /// Convert to base64 encoded string.
    /// </summary>
    /// <returns>A base64 encoded string.</returns>
    public string ToBase64()
    {
        var rawPrincipal = new RawClientPrincipal(IdentityProvider, Issuer, string.Empty, string.Empty, Claims.ToRawClaims());
        var json = JsonSerializer.SerializeToUtf8Bytes(rawPrincipal, Globals.JsonSerializerOptions);
        return Convert.ToBase64String(json);
    }

    /// <summary>
    /// Convert to a <see cref="RawClientPrincipal"/>.
    /// </summary>
    /// <returns>Converted <see cref="RawClientPrincipal"/>.</returns>
    public RawClientPrincipal ToRawClientPrincipal()
    {
        var nameType = NameType;
        if (string.IsNullOrEmpty(nameType))
        {
            nameType = Claims.FirstOrDefault(_ => _.Value == UserDetails)?.Type ?? string.Empty;
        }
        var roleType = RoleType;
        if (string.IsNullOrEmpty(roleType) && UserRoles.Any())
        {
            roleType = Claims.FirstOrDefault(_ => _.Value == UserRoles.First())?.Type ?? string.Empty;
        }
        return new RawClientPrincipal(IdentityProvider, Issuer, nameType, roleType, Claims.ToRawClaims());
    }

    /// <summary>
    /// Fetches the email address or identifier from the principal, supporting then both users and applications.
    /// </summary>
    /// <returns>The preferred_username, appid or object id.</returns>
    public string GetClientEmailOrIdentifier()
    {
        var email = Claims.FirstOrDefault(_ => _.Type == "preferred_username")?.Value;
        if (!string.IsNullOrEmpty(email))
        {
            return email;
        }

        var appId = Claims.FirstOrDefault(_ => _.Type == "appid")?.Value;
        if (!string.IsNullOrEmpty(appId))
        {
            return $"(appid) {appId}";
        }

        var oid = Claims.FirstOrDefault(_ => _.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        if (!string.IsNullOrEmpty(oid))
        {
            return $"(objectidentifier) {appId}";
        }

        var ssn = Claims.FirstOrDefault(_ => _.Type == "pid")?.Value;
        if (!string.IsNullOrEmpty(ssn))
        {
            return $"(ssn) {AnonymizeSSNForLogging(ssn)}";
        }

        var nameidentifier = Claims.FirstOrDefault(_ => _.Type.EndsWith("nameidentifier"))?.Value;
        if (!string.IsNullOrEmpty(nameidentifier))
        {
            return $"(nameidentifier) {nameidentifier}";
        }

        return string.Empty;
    }

    /// <summary>
    ///     Partially anonymize ssn so that we can log it.
    /// </summary>
    /// <param name="socialSecurityNumber">The SSN.</param>
    /// <returns>SSN with the last five digits replaced with *.</returns>
    public static string AnonymizeSSNForLogging(string socialSecurityNumber)
    {
        if (socialSecurityNumber.Length < 11)
        {
            return socialSecurityNumber;
        }

        var ssn = new StringBuilder(socialSecurityNumber);
        ssn.Remove(6, 5).Insert(6, "*****");
        return ssn.ToString();
    }
}

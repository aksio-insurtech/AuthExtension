// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Security;

/// <summary>
/// Extension methods for <see cref="IFormCollection"/>.
/// </summary>
public static class FormCollectionExtensions
{
    const string ClaimPrefix = "claim:";

    /// <summary>
    /// Converts the <see cref="IFormCollection"/> to a set of claims.
    /// </summary>
    /// <param name="form"><see cref="IFormCollection"/> to convert.</param>
    /// <returns>Collection of <see cref="Claim"/>.</returns>
    /// <remarks>
    /// This method will only convert claims that starts with <c>claim:</c>.
    /// </remarks>
    public static IEnumerable<Claim> ToClaims(this IQueryCollection form) =>
        form.Keys
            .Where(_ => _.StartsWith(ClaimPrefix))
            .Select(_ => new Claim(_.Replace(ClaimPrefix, string.Empty), form[_]))
            .ToArray();
}
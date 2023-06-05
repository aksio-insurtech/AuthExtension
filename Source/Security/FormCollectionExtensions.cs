// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Security;

public static class FormCollectionExtensions
{
    const string ClaimPrefix = "claim:";

    public static IEnumerable<Claim> ToClaims(this IFormCollection form) =>
        form.Keys
            .Where(_ => _.StartsWith(ClaimPrefix))
            .Select(_ => new Claim(_.Replace(ClaimPrefix, string.Empty), form[_]))
            .ToArray();
}
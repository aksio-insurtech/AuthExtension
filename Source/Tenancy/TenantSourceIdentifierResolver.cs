// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="ITenantSourceIdentifierResolver"/>.
/// </summary>
public class TenantSourceIdentifierResolver : ITenantSourceIdentifierResolver
{
    /// <inheritdoc/>
    public async Task<bool> CanResolve(Config config, HttpRequest request)
    {
        var genericResolverInterface = GetType().GetInterface(typeof(ITenantSourceIdentifierResolver<>).Name);
        if (genericResolverInterface is not null)
        {
            var method = genericResolverInterface.GetMethod(nameof(ITenantSourceIdentifierResolver<object>.CanResolve));
            if (method is not null)
            {
                var targetOptionsTypes = genericResolverInterface.GetGenericArguments()[0];
                var options = config.TenantResolution.Options.Deserialize(targetOptionsTypes, Globals.JsonSerializerOptions)!;
                return await (method.Invoke(this, new object[] { config, options, request }) as Task<bool>)!;
            }
        }

        return false;
    }

    /// <inheritdoc/>
    public async Task<string> Resolve(Config config, HttpRequest request)
    {
        var genericResolverInterface = GetType().GetInterface(typeof(ITenantSourceIdentifierResolver<>).Name);
        if (genericResolverInterface is not null)
        {
            var method = genericResolverInterface.GetMethod(nameof(ITenantSourceIdentifierResolver<object>.Resolve));
            if (method is not null)
            {
                var targetOptionsTypes = genericResolverInterface.GetGenericArguments()[0];
                var options = config.TenantResolution.Options.Deserialize(targetOptionsTypes, Globals.JsonSerializerOptions)!;
                return await (method.Invoke(this, new object[] { config, options, request }) as Task<string>)!;
            }
        }
        return string.Empty;
    }
}

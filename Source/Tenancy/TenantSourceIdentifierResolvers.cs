// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

public static class TenantSourceIdentifierResolvers
{
    static readonly Dictionary<TenantSourceIdentifierResolverType, ITenantSourceIdentifierResolver> _resolvers = new()
    {
        { TenantSourceIdentifierResolverType.Claim, new ClaimsSourceIdentifierResolver() },
        { TenantSourceIdentifierResolverType.Route, new RouteSourceIdentifierResolver() }
    };

    public static IServiceCollection AddTenantSourceIdentifierResolver(this IServiceCollection services)
    {
        var config = services.BuildServiceProvider().GetRequiredService<Config>();
        services.AddSingleton(_resolvers[config.TenantResolution.Strategy]);
        return services;
    }
}

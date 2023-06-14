// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Represents the different types of tenant source identifier resolvers.
/// </summary>
public static class TenantSourceIdentifierResolvers
{
    static readonly Dictionary<TenantSourceIdentifierResolverType, Type> _resolvers = new()
    {
        { TenantSourceIdentifierResolverType.None, typeof(NoneSourceIdentifierResolver) },
        { TenantSourceIdentifierResolverType.Claim, typeof(ClaimsSourceIdentifierResolver) },
        { TenantSourceIdentifierResolverType.Route, typeof(RouteSourceIdentifierResolver) }
    };

    /// <summary>
    /// Add correct tenant source identifier resolver based on configuration.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> to register with.</param>
    /// <returns><see cref="IServiceCollection"/> for continuation.</returns>
    public static IServiceCollection AddTenantSourceIdentifierResolver(this IServiceCollection services)
    {
        var config = services.BuildServiceProvider().GetRequiredService<Config>();
        services.AddSingleton(sp => sp.GetRequiredService(_resolvers[config.TenantResolution.Strategy]));
        return services;
    }
}

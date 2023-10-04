// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Tenancy;

/// <summary>
/// Source identifier helpers.
/// </summary>
public static class TenantSourceIdentifierResolverExtensions
{
    /// <summary>
    /// Verifies that the source identifier configuration seems sane, throws exception if not.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> to register with.</param>
    /// <returns><see cref="IServiceCollection"/> for continuation.</returns>
    /// <exception cref="TenantResolutionStrategyNotConfigured">Thrown if tenant resolution strategy is not properly defined.</exception>
    public static IServiceCollection VerifyTenantSourceIdentifierConfiguration(this IServiceCollection services)
    {
        var config = services.BuildServiceProvider().GetRequiredService<Config>();

        foreach (var resolver in config.TenantResolutions)
        {
            if (!Enum.IsDefined(resolver.Strategy) || resolver.Strategy == TenantSourceIdentifierResolverType.Undefined)
            {
                throw new TenantResolutionStrategyNotConfigured();
            }
        }

        return services;
    }
}
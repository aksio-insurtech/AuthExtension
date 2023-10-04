// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using MELT;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.integrationtests;

/// <summary>
/// A WebApplicationFactory spesific for ingress, which just writes the configuration-file to the expected location before any tests can run.
/// </summary>
public class IngressWebApplicationFactory : WebApplicationFactory<Program>
{
    public Config? Config;

    /// <summary>
    /// Add transient service implementations to override the real ones, to inject your testcode.
    /// </summary>
    public List<(Type Interface, object Implementation)> TransientServicesToReplace = new();

    /// <summary>
    /// Get the test logger sink.
    /// </summary>
    public ITestLoggerSink TestLoggerSink => this.GetTestLoggerSink();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (Config == null)
        {
            throw new MissingTestConfig();
        }

        builder.ConfigureLogging(logger => logger.AddTest());

        builder.ConfigureServices(
            services =>
            {
                // Inject the configuration
                var configSingleton = services.SingleOrDefault(d => d.ServiceType == typeof(Config));
                services.Remove(configSingleton);
                services.AddSingleton(Config);

                // // And then we have to set the tenant resolver again (as it is set up through Program.cs before we get here, and can override the config)
                // var resolver = services.SingleOrDefault(d => d.ServiceType == typeof(ISourceIdentifierResolver));
                // services.Remove(resolver);
                // services.AddTenantSourceIdentifierResolver();
                
                foreach (var testService in TransientServicesToReplace)
                {
                    var service = services.SingleOrDefault(d => d.ServiceType == testService.Interface);
                    services.Remove(service);
                    services.Add(new(testService.Interface, testService.Implementation));
                }
            });
    }
}
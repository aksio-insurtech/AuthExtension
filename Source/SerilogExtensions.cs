// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Applications.Serilog;
using Serilog;
using Serilog.Exceptions;

namespace Aksio.IngressMiddleware;

/// <summary>
/// Temporarily stored here, before it is accepted into the ApplicationModel repo.
/// </summary>
public static class SerilogExtensions
{
    /// <summary>
    /// Use default logging.
    /// </summary>
    /// <param name="builder"><see creF="IHostBuilder"/> to use with.</param>
    /// <returns><see creF="IHostBuilder"/> for continuation.</returns>
    public static ILoggerFactory UseDefaultLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration().Enrich.WithExceptionDetails()
            .Enrich.WithExecutionContext()
            .ReadFrom.Configuration(ConfigurationHostBuilderExtensions.Configuration)
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger);

        Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

        return new Serilog.Extensions.Logging.SerilogLoggerFactory();
    }
}
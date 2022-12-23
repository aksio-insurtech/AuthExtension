// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
var loggerFactory = builder.Host.UseDefaultLogging();
var app = builder.Build();
app.UseStaticFiles();

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("./config/config.json", optional: true, reloadOnChange: true);
var configuration = configurationBuilder.Build();

Globals.Logger = loggerFactory.CreateLogger("Default");
Globals.Logger.LogInformation("Setting up routes");

AppDomain.CurrentDomain.UnhandledException += UnhandledExceptions;

var httpClientFactory = app.Services.GetService<IHttpClientFactory>()!;

app.MapGet("/", async (HttpRequest request, HttpResponse response) =>
{
    var config = configuration.Get<Config>();
    await Cratis.HandleRequest(config, request, response);
    await Identity.HandleRequest(config, request, response, httpClientFactory);
});

app.MapGet("/id-porten/authorize/", async (HttpRequest request, HttpResponse response) =>
{
    var config = configuration.Get<Config>();
    await IdPorten.HandleAuthorize(config, request, response);
});

app.Run();

static void UnhandledExceptions(object sender, UnhandledExceptionEventArgs args)
{
    if (args.ExceptionObject is Exception exception)
    {
        Log.Logger?.Error(exception, "Unhandled exception");
        Log.CloseAndFlush();
        Console.WriteLine("************ BEGIN UNHANDLED EXCEPTION ************");
        PrintExceptionInfo(exception);

        while (exception.InnerException != null)
        {
            Console.WriteLine("\n------------ BEGIN INNER EXCEPTION ------------");
            PrintExceptionInfo(exception.InnerException);
            exception = exception.InnerException;
            Console.WriteLine("------------ END INNER EXCEPTION ------------\n");
        }

        Console.WriteLine("************ END UNHANDLED EXCEPTION ************ ");
    }
}

static void PrintExceptionInfo(Exception exception)
{
    Console.WriteLine($"Exception type: {exception.GetType().FullName}");
    Console.WriteLine($"Exception message: {exception.Message}");
    Console.WriteLine($"Stack Trace: {exception.StackTrace}");
}

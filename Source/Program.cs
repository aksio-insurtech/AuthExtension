// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json;
using Aksio.IngressMiddleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
var loggerFactory = builder.Host.UseDefaultLogging();
var app = builder.Build();
app.UseStaticFiles();

const string configFile = "./config/config.json";

var config = new Config();
if (File.Exists(configFile))
{
    var configJson = File.ReadAllText(configFile);
    config = JsonSerializer.Deserialize<Config>(configJson, Globals.JsonSerializerOptions)!;
}

Globals.Logger = loggerFactory.CreateLogger("Default");
Globals.Logger.LogInformation("Setting up routes");

AppDomain.CurrentDomain.UnhandledException += UnhandledExceptions;

var httpClientFactory = app.Services.GetService<IHttpClientFactory>()!;

app.MapGet("/", async (HttpRequest request, HttpResponse response) =>
{
    var tenantId = await Tenancy.HandleRequest(config, request, response);

    // TODO: Impersonation. Look for impersonation cookie, if present, use that as the principal

    await Identity.HandleRequest(config, request, response, tenantId, httpClientFactory);
    await OAuthBearerTokens.HandleRequest(config, request, response, tenantId, httpClientFactory);
});

app.MapGet("/id-porten/authorize/", (HttpRequest request, HttpResponse response) =>
    IdPorten.HandleAuthorize(config, request, response));

app.MapPost("/.aksio/impersonate", (HttpRequest request, HttpResponse response) =>
{
    // Pull out claims from the FORM 
    // Add claims to a structure
    // Add an impersonation cookie
    Console.WriteLine("Hello world");
});

app.MapGet("/.aksio/impersonate/auth", (HttpRequest request, HttpResponse response) =>
{
    var principalBase64 = request.Headers[Headers.Principal];
    var principalJson = Convert.FromBase64String(principalBase64.ToString());
    var principalString = Encoding.UTF8.GetString(principalJson);
    var principal = JsonSerializer.Deserialize<ClientPrincipal>(principalJson, Globals.JsonSerializerOptions);

    // Check if tenant is allowed
    // Check if Identity Provider is allowed
    // Check if a claim is allowed (Group membership)

    Console.WriteLine("Hello Auth");
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

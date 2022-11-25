// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware;

var builder = WebApplication.CreateBuilder(args);
var loggerFactory = builder.Host.UseDefaultLogging();
var app = builder.Build();

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("./config/config.json", optional: true, reloadOnChange: true);
var configuration = configurationBuilder.Build();

Globals.Logger = loggerFactory.CreateLogger("Default");
Globals.Logger.LogInformation("Setting up routes");

app.MapGet("/", async (HttpRequest request, HttpResponse response) =>
{
    var config = configuration.Get<Config>();
    await Cratis.HandleRequest(config, request, response);
});

app.MapGet("/id-porten/authorize/", async (HttpRequest request, HttpResponse response) =>
{
    var config = configuration.Get<Config>();
    await IdPorten.HandleAuthorize(config, request, response);
});

app.MapGet("/id-porten/.well-known/openid-configuration", async (HttpRequest request, HttpResponse response) =>
{
    var config = configuration.Get<Config>();
    await IdPorten.HandleWellKnownConfiguration(config, request, response);
});

app.Run();

// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("./config/config.json", optional: true, reloadOnChange: true);
var configuration = configurationBuilder.Build();

app.MapGet("/", async (HttpRequest request, HttpResponse response) =>
{
    var config = configuration.Get<Config>();
    await Cratis.HandleRequest(config, request, response);
    await AzureContainerAppAuth.HandleZumoHeader(config, request, response);
});

app.MapGet("/aad/authorize", async (HttpRequest request, HttpResponse response) =>
{
    var config = configuration.Get<Config>();
    await AzureAd.HandleAuthorize(config, request, response);
});

app.MapGet("/aad/login/callback", async (HttpRequest request, HttpResponse response) =>
{
    var config = configuration.Get<Config>();
    await AzureAd.HandleCallback(config, request, response);
});

app.MapGet("/aad/.well-known/openid-configuration", async (HttpRequest request, HttpResponse response) =>
{
    var config = configuration.Get<Config>();
    await AzureAd.HandleWellKnownConfiguration(config, request, response);
});

app.MapGet("/id-porten/authorize/", async (HttpRequest request, HttpResponse response) =>
{
    var config = configuration.Get<Config>();
    await IdPorten.HandleAuthorize(config, request, response);
});

app.MapGet("/id-porten/login/callback", async (HttpRequest request, HttpResponse response) =>
{
    var config = configuration.Get<Config>();
    await IdPorten.HandleCallback(config, request, response);
});

app.Run();

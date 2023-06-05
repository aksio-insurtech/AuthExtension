// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware;
using Aksio.IngressMiddleware.Configuration;

UnhandledExceptionsManager.Setup();

var config = Config.Load();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddSingleton(config);

builder.Services.AddHttpClient();
var loggerFactory = builder.Host.UseDefaultLogging();
Globals.Logger = loggerFactory.CreateLogger("Default");
Globals.Logger.LogInformation("Setting up routes");

var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.UseStaticFiles();

var httpClientFactory = app.Services.GetService<IHttpClientFactory>()!;

app.MapGet("/", async (HttpRequest request, HttpResponse response) =>
{
    var tenantId = await Tenancy.HandleRequest(config, request, response);

    // TODO: Impersonation. Look for impersonation cookie, if present, use that as the principal

    await Identity.HandleRequest(config, request, response, tenantId, httpClientFactory);
    await OAuthBearerTokens.HandleRequest(config, request, response, tenantId, httpClientFactory);
});

app.Run();

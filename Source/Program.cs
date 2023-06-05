// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware;
using Aksio.IngressMiddleware.BearerTokens;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Identities;
using Aksio.IngressMiddleware.Tenancy;

UnhandledExceptionsManager.Setup();

var config = Config.Load();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddSingleton(config);
builder.Services.AddTransient<ITenantResolver, TenantResolver>();
builder.Services.AddTransient<IIdentityDetailsResolver, IdentityDetailsResolver>();
builder.Services.AddTransient<IOAuthBearerTokens, OAuthBearerTokens>();

builder.Services.AddHttpClient();
var loggerFactory = builder.Host.UseDefaultLogging();

var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.UseStaticFiles();

app.Run();

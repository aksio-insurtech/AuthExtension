// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.NginxMiddleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async (HttpRequest request, HttpResponse response) =>
{
    await Trudesk.HandleRequest(request, response);
    await Cratis.HandleRequest(request, response);
});

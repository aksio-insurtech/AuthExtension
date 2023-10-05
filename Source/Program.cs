// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware;
using Aksio.IngressMiddleware.BearerTokens;
using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Identities;
using Aksio.IngressMiddleware.Impersonation;
using Aksio.IngressMiddleware.MutualTLS;
using Aksio.IngressMiddleware.RoleAuthorization;
using Aksio.IngressMiddleware.Tenancy;
using Aksio.IngressMiddleware.Tenancy.SourceIdentifierResolvers;
using Aksio.Types;

#pragma warning disable CA1050, MA0047, MA0036

UnhandledExceptionsManager.Setup();

var config = Config.Load();

var builder = WebApplication.CreateBuilder(args);

#pragma warning disable CA2000 // Dispose objects before losing scope => Disposed by the host
builder.UseDefaultLogging();
#pragma warning restore CA2000

builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddSingleton(config);
builder.Services.AddTransient<ITenantResolver, TenantResolver>();
builder.Services.AddTransient<IIdentityDetailsResolver, IdentityDetailsResolver>();
builder.Services.AddTransient<IOAuthBearerTokenValidator, OAuthBearerTokenValidator>();
builder.Services.AddTransient<IOAuthBearerTokens, OAuthBearerTokens>();
builder.Services.AddTransient<IMutualTLS, MutualTLS>();
builder.Services.AddTransient<IRoleAuthorizer, RoleAuthorizer>();
builder.Services.AddTransient<TenantImpersonationAuthorizer>();
builder.Services.AddTransient<IdentityProviderImpersonationAuthorizer>();
builder.Services.AddTransient<ClaimImpersonationAuthorizer>();
builder.Services.AddTransient<RolesImpersonationAuthorizer>();
builder.Services.AddTransient<GroupsImpersonationAuthorizer>();
builder.Services.AddSingleton<IImpersonationFlow, ImpersonationFlow>();

foreach (var sourceIdentifier in Types.Instance.FindMultiple<ISourceIdentifier>())
{
    builder.Services.AddSingleton(typeof(ISourceIdentifier), sourceIdentifier);
}
builder.Services.AddSingleton<ISourceIdentifierResolver, SourceIdentifierResolver>();
builder.Services.VerifyTenantSourceIdentifierConfiguration();

builder.Services.AddHttpClient();

var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.UseStaticFiles();

app.Run();

/// <summary>
/// Make Program class public, so that it can be used for integration testing.
/// </summary>
#pragma warning disable SA1502, MA0036
public partial class Program { }
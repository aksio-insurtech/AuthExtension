// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;

namespace Aksio.IngressMiddleware.Identities;

public interface IIdentityDetailsResolver
{
    Task<bool> Resolve(HttpRequest request, HttpResponse response, TenantId tenantId);
}


// public static class Identity
// {
//     const string CookieName = ".aksio-identity";

//     public static async Task HandleRequest(Config config, HttpRequest request, HttpResponse response, TenantId tenantId, IHttpClientFactory httpClientFactory)
//     {
//         if (string.IsNullOrEmpty(config.IdentityDetailsUrl))
//         {
//             Globals.Logger.LogInformation("Identity details url is not configured, skipping identity details resolution");
//             return;
//         }

//         if (!request.Cookies.ContainsKey(CookieName)
//             && request.Headers.ContainsKey(Headers.Principal))
//         {
//             try
//             {
//                 var client = httpClientFactory.CreateClient();
//                 var principalId = string.Empty;
//                 var principalName = string.Empty;

//                 if (request.Headers.ContainsKey(Headers.PrincipalId))
//                 {
//                     principalId = request.Headers[Headers.PrincipalId];
//                 }
//                 if (request.Headers.ContainsKey(Headers.PrincipalName))
//                 {
//                     principalName = request.Headers[Headers.PrincipalName];
//                 }

//                 if (string.IsNullOrEmpty(principalId))
//                 {
//                     principalId = "[NotSet]";
//                 }
//                 if (string.IsNullOrEmpty(principalName))
//                 {
//                     principalName = "[NotSet]";
//                 }

//                 Globals.Logger.LogInformation("Resolving identity details for {PrincipalId} and {TenantId}", principalId, tenantId);

//                 client.DefaultRequestHeaders.Add(Headers.Principal, request.Headers[Headers.Principal].ToString());
//                 client.DefaultRequestHeaders.Add(Headers.PrincipalId, principalId);
//                 client.DefaultRequestHeaders.Add(Headers.PrincipalName, principalName);
//                 client.DefaultRequestHeaders.Add(Headers.TenantId, tenantId.ToString());
//                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//                 client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
//                 client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));

//                 var responseMessage = await client.GetAsync(config.IdentityDetailsUrl);
//                 if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
//                 {
//                     response.StatusCode = 403;
//                 }
//                 var identityDetails = await responseMessage.Content.ReadAsStringAsync();

//                 if (responseMessage.StatusCode != HttpStatusCode.OK)
//                 {
//                     Globals.Logger.LogError("Error trying to resolve identity details for {PrincipalId} on tenant {TenantId}: {StatusCode} {ReasonPhrase}", principalId, tenantId, responseMessage.StatusCode, responseMessage.ReasonPhrase);
//                     return;
//                 }

//                 var encoding = Encoding.GetEncoding("iso-8859-1");
//                 var encoded = encoding.GetBytes(identityDetails);
//                 var identityDetailsAsBase64 = Convert.ToBase64String(encoded);
//                 response.Cookies.Append(CookieName, identityDetailsAsBase64, new CookieOptions { Expires = DateTimeOffset.Now.AddMinutes(5) });
//             }
//             catch (Exception ex)
//             {
//                 Globals.Logger.LogError(ex, "Error trying to resolve identity details");
//             }
//         }
//     }
// }
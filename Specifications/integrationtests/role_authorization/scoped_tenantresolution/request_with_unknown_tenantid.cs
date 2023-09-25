// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.integrationtests.role_authorization.given;

namespace Aksio.IngressMiddleware.integrationtests.role_authorization.scoped_tenantresolution;

/// <summary>
/// Test scenario with Entra ID done on Azure Container App ingress, just passing claims to get tenant verified.
/// </summary>
public class request_with_unknown_tenantid : factory_with_role_auth_with_scoped_tenancyresolution
{
    HttpResponseMessage _responseMessage;

    async Task Because()
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/");
        BuildAndSetPrincipalWithTenantClaim(requestMessage, "unknowntenant", AcceptedRoles.First());

        _responseMessage = await IngressClient.SendAsync(requestMessage);
    }

    [Fact]
    void access_denied() => _responseMessage.IsSuccessStatusCode.ShouldBeFalse();
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.integrationtests.role_authorization.given;

namespace Aksio.IngressMiddleware.integrationtests.role_authorization.scoped_tenantresolution;

public class request_with_host_resolution_and_valid_entraid_tenant : factory_with_host_resolution_and_role_auth_with_scoped_tenancyresolution
{
    HttpResponseMessage _responseMessage;

    async Task Because()
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://host0001/");
        BuildAndSetPrincipalWithTenantClaim(
            requestMessage,
            EntraId1,
            AudienceWithRoles,
            "user@entraid1.com",
            AcceptedRolesPrAudience[AudienceWithRoles].Roles.ToArray());

        _responseMessage = await IngressClient.SendAsync(requestMessage);
    }

    [Fact]
    void access_granted() => _responseMessage.IsSuccessStatusCode.ShouldBeTrue();
}
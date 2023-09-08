// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.integrationtests.oauth_bearertoken.given;

namespace Aksio.IngressMiddleware.integrationtests.oauth_bearertoken.scoped_tenantresolution;

public class valid_route_unknown_sourceidentifier : factory_with_oauth_bearertoken_auth_with_route_tenancyresolution
{
    HttpResponseMessage _responseMessage;

    async Task Because()
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/");
        SetOriginalUrlAndAuthHeaders(requestMessage, "/9999/sdlfkjsdf");

        _responseMessage = await IngressClient.SendAsync(requestMessage);
    }

    [Fact]
    void access_denied() => _responseMessage.IsSuccessStatusCode.ShouldBeFalse();
}
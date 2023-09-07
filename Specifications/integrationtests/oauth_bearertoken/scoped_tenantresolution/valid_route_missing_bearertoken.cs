// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.integrationtests.oauth_bearertoken.given;

namespace Aksio.IngressMiddleware.integrationtests.oauth_bearertoken.scoped_tenantresolution;

public class valid_route_missing_bearertoken : factory_with_oauth_bearertoken_auth_with_route_tenancyresolution
{
    HttpResponseMessage _responseMessage;

    void Establish() => IsTokenValid = false;

    async Task Because()
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/");
        requestMessage.Headers.Add(Headers.OriginalUri, "/2345/somethingelse");

        _responseMessage = await IngressClient.SendAsync(requestMessage);
    }

    [Fact]
    void access_denied() => _responseMessage.IsSuccessStatusCode.ShouldBeFalse();
}
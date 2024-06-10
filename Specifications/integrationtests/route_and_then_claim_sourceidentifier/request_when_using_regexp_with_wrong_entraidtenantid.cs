// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.integrationtests.route_and_then_claim_sourceidentifier.given;

namespace Aksio.IngressMiddleware.integrationtests.route_and_then_claim_sourceidentifier;

public class request_when_using_regexp_with_wrong_entraidtenantid : multi_resolution_host
{
    HttpResponseMessage _responseMessage;

    async Task Because()
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/");
        BuildAndSetPrincipalWithTenantClaim(requestMessage, Guid.NewGuid().ToString(), "audienceWithNoAuth");

        requestMessage.Headers.Add(Headers.OriginalUri, $"/{ExpectedRouteSourceIdentifier}/blahblah");

        _responseMessage = await _ingressClient.SendAsync(requestMessage);
    }

    [Fact]
    void access_denied() => _responseMessage.IsSuccessStatusCode.ShouldBeFalse();

    [Fact]
    void got_the_expected_tenant() =>
        _responseMessage.Headers.GetValues("Tenant-ID").FirstOrDefault().ShouldEqual(ExpectedRouteTenantId.ToString());
}
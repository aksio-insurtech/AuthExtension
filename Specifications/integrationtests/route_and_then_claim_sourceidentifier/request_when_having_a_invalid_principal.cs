// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.integrationtests.route_and_then_claim_sourceidentifier.given;

namespace Aksio.IngressMiddleware.integrationtests.route_and_then_claim_sourceidentifier;

public class request_when_having_a_invalid_principal : multi_resolution_host
{
    HttpResponseMessage _responseMessage;

    async Task Because()
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/");
        BuildAndSetPrincipalWithTenantClaim(requestMessage, "invalid_entraid_tenantid", "audienceWithNoAuth");

        requestMessage.Headers.Add(Headers.OriginalUri, "/not/a/regexp/matched/url");

        _responseMessage = await _ingressClient.SendAsync(requestMessage);
    }

    [Fact]
    void access_granted() => _responseMessage.IsSuccessStatusCode.ShouldBeFalse();
}
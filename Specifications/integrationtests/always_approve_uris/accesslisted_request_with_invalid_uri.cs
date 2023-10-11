// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.integrationtests.always_approve_uris.given;

namespace Aksio.IngressMiddleware.integrationtests.always_approve_uris;

/// <summary>
/// Test scenario mused with with Idporten flow: pre approved uri.
/// </summary>
public class accesslisted_request_with_invalid_uri : factory_with_approveduris
{
    HttpResponseMessage _responseMessage;

    async Task Because()
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://{ApprovedHost}/");
        requestMessage.Headers.Add(Headers.OriginalUri, $"/some/other/exciting/thing");
        
        _responseMessage = await IngressClient.SendAsync(requestMessage);
    }

    [Fact]
    void access_denied() => _responseMessage.IsSuccessStatusCode.ShouldBeFalse();
}



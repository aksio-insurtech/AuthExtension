// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.integrationtests.mtls_auth.given;

namespace Aksio.IngressMiddleware.integrationtests.mtls_auth.scoped_tenantresolution;

/// <summary>
/// Test scenario with Entra ID done on Azure Container App ingress, just passing claims to get tenant verified.
/// </summary>
public class request_with_unknown_clientcert : factory_with_mtls_auth
{
    HttpResponseMessage _responseMessage;

    async Task Because()
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/");
        SetupClientCertRequestHeader(requestMessage, UnknownClientCert);

        _responseMessage = await IngressClient.SendAsync(requestMessage);
    }

    [Fact]
    void access_denied() => _responseMessage.IsSuccessStatusCode.ShouldBeFalse();
}
using Aksio.IngressMiddleware.integrationtests.route_and_then_claim_sourceidentifier.given;

namespace Aksio.IngressMiddleware.integrationtests.route_and_then_claim_sourceidentifier;

public class request_when_falling_over_to_principal : multi_resolution_host
{
    HttpResponseMessage _responseMessage;

    async Task Because()
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/");
        BuildAndSetPrincipalWithTenantClaim(requestMessage, ExpectedEntraIdTenantSourceIdentifier, "audienceWithNoAuth");

        requestMessage.Headers.Add(Headers.OriginalUri, "/not/a/regexp/matched/url");

        _responseMessage = await _ingressClient.SendAsync(requestMessage);
    }

    [Fact]
    void access_granted() => _responseMessage.IsSuccessStatusCode.ShouldBeTrue();

    [Fact]
    void got_the_expected_tenant() =>
        _responseMessage.Headers.GetValues("Tenant-ID").FirstOrDefault().ShouldEqual(ExpectedEntraIdTenantId.ToString());
}
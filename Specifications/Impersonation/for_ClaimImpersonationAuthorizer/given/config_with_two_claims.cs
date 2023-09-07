// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware.Impersonation.for_ClaimImpersonationAuthorizer.when_asking_if_authorized.given;

public class config_with_two_claims : Aksio.IngressMiddleware.Impersonation.given.a_http_context
{
    protected const string first_claim_type = "first_claim";
    protected const string first_claim_value = "first_claim_value";
    protected const string second_claim_type = "second_claim";
    protected const string second_claim_value = "second_claim_value";
    protected Config config;

    void Establish()
    {
        config = new();
        config.Impersonation.Authorization.Claims = new[]
        {
            new Claim(first_claim_type, first_claim_value),
            new Claim(second_claim_type, second_claim_value)
        };
    }
}

public class config_with_no_claims : Aksio.IngressMiddleware.Impersonation.given.a_http_context
{
    protected Config config;

    void Establish() => config = new();
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Aksio.IngressMiddleware.given;
using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware.Impersonation.for_ClaimImpersonationAuthorizer.given;

public class config_with_two_claims : a_http_context
{
    protected const string FirstClaimType = "first_claim";
    protected const string FirstClaimValue = "first_claim_value";
    protected const string SecondClaimType = "second_claim";
    protected const string SecondClaimValue = "second_claim_value";
    protected Config Config;

    void Establish()
    {
        Config = new();
        Config.Impersonation.Authorization.Claims = new[]
        {
            new Claim(FirstClaimType, FirstClaimValue),
            new Claim(SecondClaimType, SecondClaimValue)
        };
    }
}

public class config_with_no_claims : a_http_context
{
    protected Config Config;

    void Establish() => Config = new();
}
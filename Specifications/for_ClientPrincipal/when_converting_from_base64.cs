// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Aksio.IngressMiddleware;

public class when_converting_from_base64 : Specification
{
    const string IdentityProvider = "aad";
    const string UserId = "1234";
    const string UserName = "test user";
    const string UserRole = "user";
    const string AdminRole = "admin";

    string input = $"{{\"auth_typ\":\"{IdentityProvider}\",\"name_typ\":\"name\",\"role_typ\":\"roles\",\"claims\":[{{\"typ\":\"name\",\"val\":\"{UserName}\"}},{{\"typ\":\"roles\",\"val\":\"user\"}}, {{\"typ\":\"roles\",\"val\":\"admin\"}}]}}";

    ClientPrincipal result;

    void Because() => result = ClientPrincipal.FromBase64(UserId, Convert.ToBase64String(Encoding.UTF8.GetBytes(input)));

    [Fact] void should_have_the_correct_identity_provider() => result.IdentityProvider.ShouldEqual(IdentityProvider);
}
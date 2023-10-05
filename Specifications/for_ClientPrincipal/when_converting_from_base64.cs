// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Aksio.IngressMiddleware.for_ClientPrincipal;

public class when_converting_from_base64 : Specification
{
    const string IdentityProvider = "aad";
    const string UserId = "1234";
    const string UserName = "test user";
    const string UserRole = "user";
    const string AdminRole = "admin";

    string _input =
        $"{{\"auth_typ\":\"{IdentityProvider}\",\"name_typ\":\"name\",\"role_typ\":\"roles\",\"claims\":[{{\"typ\":\"name\",\"val\":\"{UserName}\"}},{{\"typ\":\"roles\",\"val\":\"user\"}}, {{\"typ\":\"roles\",\"val\":\"admin\"}}]}}";

    ClientPrincipal _result;

    void Because() => _result = ClientPrincipal.FromBase64(UserId, Convert.ToBase64String(Encoding.UTF8.GetBytes(_input)));

    [Fact]
    void should_have_the_correct_identity_provider() => _result.IdentityProvider.ShouldEqual(IdentityProvider);

    [Fact]
    void should_have_the_correct_user_id() => _result.UserId.ShouldEqual(UserId);

    [Fact]
    void should_have_the_correct_user_name() => _result.UserDetails.ShouldEqual(UserName);

    [Fact]
    void should_have_the_correct_roles() => _result.UserRoles.ShouldContain(UserRole, AdminRole);

    [Fact]
    void should_hold_the_name_claim() => _result.Claims.ShouldContain(claim => claim.Type == "name" && claim.Value == UserName);

    [Fact]
    void should_hold_the_user_role_claim() =>
        _result.Claims.ShouldContain(claim => claim.Type == "roles" && claim.Value == UserRole);

    [Fact]
    void should_hold_the_admin_role_claim() =>
        _result.Claims.ShouldContain(claim => claim.Type == "roles" && claim.Value == AdminRole);
}
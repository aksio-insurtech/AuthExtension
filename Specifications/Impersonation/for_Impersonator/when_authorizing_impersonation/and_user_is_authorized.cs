// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.Impersonation.for_Impersonator.when_authorizing_impersonation;

public class and_user_is_authorized : given.a_principal
{
    IActionResult _result;

    void Establish() =>
        Authorizer.Setup(_ => _.IsAuthorized(Impersonator.ControllerContext.HttpContext.Request, IsAny<ClientPrincipal>()))
            .ReturnsAsync(true);

    async Task Because() => _result = await Impersonator.Authorize();

    [Fact]
    void should_return_ok() => _result.ShouldBeOfExactType<OkResult>();
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_MutualTLS;

public class when_handling_a_valid_mtls_client : given.a_mutualtls_instance
{
    IActionResult _result;

    void Because() => _result = MutualTls.Handle(Request);

    [Fact]
    void should_accept_client() => _result.ShouldBeOfExactType<OkResult>();
}
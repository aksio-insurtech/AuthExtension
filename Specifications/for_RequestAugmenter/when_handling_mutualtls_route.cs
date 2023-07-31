// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_RequestAugmenter;

public class when_handling_enabled_mutualtls : given.a_request_augmenter
{
    void Establish()
    {
        mutual_tls.Setup(_ => _.IsEnabled()).Returns(true);
        mutual_tls.Setup(_ => _.Handle(request)).Returns(new OkResult());
    }

    Task Because() => augmenter.Get();

    [Fact]
    void should_check_to_call_mutualtls_handler() =>
        mutual_tls.Verify(_ => _.IsEnabled(), Once);

    [Fact]
    void should_call_mutualtls_handler() =>
        mutual_tls.Verify(_ => _.Handle(request), Once);
}
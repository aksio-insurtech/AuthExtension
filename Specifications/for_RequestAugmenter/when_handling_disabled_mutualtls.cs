// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;

namespace Aksio.IngressMiddleware.for_RequestAugmenter;

public class when_handling_disabled_mutualtls : given.a_request_augmenter
{
    void Establish()
    {
        MutualTls.Setup(_ => _.IsEnabled()).Returns(false);
        MutualTls.Setup(_ => _.Handle(Request)).Returns(new OkResult());
    }

    Task Because() => Augmenter.Get();

    [Fact]
    void should_check_to_call_mutualtls_handler() => MutualTls.Verify(_ => _.IsEnabled(), Once);

    [Fact]
    void should_not_call_mutualtls_handler() => MutualTls.Verify(_ => _.Handle(Request), Never);
}
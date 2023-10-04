// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.Impersonation.for_ImpersonationFlow.given;

public class a_impersonation_flow : Specification
{
    protected ImpersonationFlow Flow;
    protected Config Config;

    void Establish()
    {
        Config = new();
        Flow = new(Config, Mock.Of<ILogger<ImpersonationFlow>>());
    }
}
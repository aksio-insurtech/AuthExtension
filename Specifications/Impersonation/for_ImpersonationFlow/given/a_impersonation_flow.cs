// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.Impersonation.given;

public class a_impersonation_flow : Specification
{
    protected ImpersonationFlow flow;
    protected Config config;

    void Establish()
    {
        config = new Config();
        flow = new(config);
    }
}
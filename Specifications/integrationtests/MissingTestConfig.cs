// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.integrationtests;

public class MissingTestConfig : Exception
{
    public MissingTestConfig()
        : base($"You must set the Config property on the {nameof(IngressWebApplicationFactory)} before starting the test")
    {
    }
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.integrationtests.always_approve_uris.given;

public class factory_with_approveduris : Specification
{
    protected IngressWebApplicationFactory IngressFactory;
    protected HttpClient IngressClient;
    protected Config IngressConfig;
    protected string ApprovedHost = "some_host";
    protected string ApprovedUri = "/the/endpoint/thingie";

    void Establish()
    {
        IngressConfig = new()
        {
            AlwaysApproveUris = new List<string>()
            {
                $"{ApprovedHost}{ApprovedUri}"
            }
        };

        IngressFactory = new()
        {
            Config = IngressConfig
        };

        IngressClient = IngressFactory.CreateClient();
    }
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.Impersonation.given;

public class a_http_context : Specification
{
    protected DefaultHttpContext http_context;

    void Establish()
    {
        http_context = new();
    }
}
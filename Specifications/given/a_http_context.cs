// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Aksio.IngressMiddleware.given;

public class a_http_context : Specification
{
    protected DefaultHttpContext HttpContext;

    void Establish() => HttpContext = new();
}
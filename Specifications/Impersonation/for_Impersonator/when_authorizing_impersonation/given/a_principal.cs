// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Impersonation.for_Impersonator.when_authorizing_impersonation.given;

public class a_principal : an_impersonator
{
    void Establish() => impersonator.ControllerContext.HttpContext.Request.Headers.Add(Headers.Principal, "e30=");
}
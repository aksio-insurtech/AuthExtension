// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Impersonation;

[Route("/.aksio/impersonate")]
public class Impersonation : Controller
{
    [HttpPost]
    public IActionResult Impersonate()
    {
        // Pull out claims from the FORM 
        // Add claims to a structure
        // Add an impersonation cookie

        return Ok();
    }


    [HttpGet("auth")]
    public IActionResult Auth()
    {
        var principal = ClientPrincipal.FromBase64(Request.Headers[Headers.Principal]);

        // Check if tenant is allowed
        // Check if Identity Provider is allowed
        // Check if a claim is allowed (Group membership)

        return Ok();
    }
}

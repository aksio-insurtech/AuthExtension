// // Copyright (c) Aksio Insurtech. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.
//
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Aksio.IngressMiddleware.for_RequestAugmenter;
//
// public class when_handling_with_unsupported_tenant : given.a_request_augmenter
// {
//     IActionResult _result;
//
//     void Establish() => TenantResolver.Setup(_ => _.CanResolve(IsAny<HttpRequest>())).ReturnsAsync(false);
//
//     async Task Because() => _result = await Augmenter.Get();
//
//     [Fact]
//     void should_return_unauthorized() => ((StatusCodeResult)_result).StatusCode.ShouldEqual(StatusCodes.Status401Unauthorized);
// }
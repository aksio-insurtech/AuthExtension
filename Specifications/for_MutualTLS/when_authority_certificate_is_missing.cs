// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.for_MutualTLS;

public class when_authority_certificate_is_missing : given.a_mutual_tls_instance
{
    IActionResult _result;

    void Establish() => MutualTlsConfig.AuthorityCertificate = "not a real certificate";

    void Because() => _result = MutualTls.Handle(Request);

    [Fact]
    void should_return_forbidden() => ((StatusCodeResult)_result).StatusCode.ShouldEqual(StatusCodes.Status401Unauthorized);

    [Fact]
    void should_log_access_denied() =>
        LoggerFactory.Sink.LogEntries.ShouldContain(_ => _.EventId == 2 && _.LogLevel == LogLevel.Error);
}
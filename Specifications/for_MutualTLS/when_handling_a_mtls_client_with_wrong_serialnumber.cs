// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.for_MutualTLS;

public class when_handling_a_mtls_client_with_wrong_serialnumber : given.a_mutual_tls_instance
{
    IActionResult _result;

    void Establish() => MutualTlsConfig.AcceptedSerialNumbers = new List<string> { "1" };

    void Because() => _result = MutualTls.Handle(Request);

    [Fact]
    void should_return_forbidden() => ((StatusCodeResult)_result).StatusCode.ShouldEqual(StatusCodes.Status401Unauthorized);

    [Fact]
    void should_log_the_attempted_serialnumber() =>
        LoggerFactory.Sink.LogEntries.ShouldContain(_ => _.EventId == 5 && _.LogLevel == LogLevel.Error && _.Properties.Any(p => (p.Value as string) == ClientCertificateSerial.ToUpper()));
}

// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;
using Aksio.IngressMiddleware.integrationtests.entraid_auth.given;
using MELT;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.integrationtests.entraid_auth.scoped_tenantresolution;

public class request_with_valid_roles : factory_with_entraid_auth_with_scoped_tenancyresolution
{
    HttpResponseMessage _responseMessage;
    List<LogEntry> _logEntries;

    async Task Because()
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/");
        BuildAndSetPrincipalWithTenantClaim(
            requestMessage,
            IngressConfig.Tenants.Values.Last().SourceIdentifiers.Last(),
            AcceptedRoles.First(),
            AcceptedRoles.Last());

        _responseMessage = await IngressClient.SendAsync(requestMessage);

        _logEntries = IngressFactory.TestLoggerSink.LogEntries
            .Where(l => l.LoggerName == typeof(EntraIdRoles.EntraIdRoles).FullName)
            .ToList();
    }

    [Fact]
    void should_be_authorized() => _responseMessage.StatusCode.ShouldEqual(HttpStatusCode.OK);

    [Fact]
    void the_attempt_was_logged() => _logEntries.ShouldContain(l => l.EventId == 2 && l.LogLevel == LogLevel.Information);
}
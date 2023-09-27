// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;
using Aksio.IngressMiddleware.integrationtests.role_authorization.given;
using Aksio.IngressMiddleware.RoleAuthorization;
using MELT;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.integrationtests.role_authorization.scoped_tenantresolution;

public class request_with_no_roles_required : factory_with_role_auth_with_scoped_tenancyresolution
{
    HttpResponseMessage _responseMessage;
    List<LogEntry> _logEntries;

    async Task Because()
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/");
        BuildAndSetPrincipalWithTenantClaim(requestMessage, IngressConfig.Tenants.Values.Last().SourceIdentifiers.Last(), AudienceWithNoAuthRequired);

        _responseMessage = await IngressClient.SendAsync(requestMessage);

        _logEntries = IngressFactory.TestLoggerSink.LogEntries
            .Where(l => l.LoggerName == typeof(RoleAuthorizer).FullName)
            .ToList();
    }

    [Fact]
    void should_be_authorized() => _responseMessage.StatusCode.ShouldEqual(HttpStatusCode.OK);

    [Fact]
    void the_attempt_was_logged() => _logEntries.ShouldContain(l => l.EventId == 3 && l.LogLevel == LogLevel.Information);
}
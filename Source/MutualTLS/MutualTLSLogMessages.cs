// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.MutualTLS;

static partial class MutualTLSLogMessages
{
    [LoggerMessage(
        0,
        LogLevel.Error,
        "Mutual TLS/mTLS client certificate request is missing the 'x-forwarded-client-cert' header, client address = {ClientIp}")]
    internal static partial void MissingHeader(this ILogger<MutualTLS> logger, string clientIp);

    [LoggerMessage(1, LogLevel.Error, "Missing certificate in the 'x-forwarded-client-cert' header, client address = {ClientIp}")]
    internal static partial void MissingCertificateInHeader(this ILogger<MutualTLS> logger, string clientIp);

    [LoggerMessage(2, LogLevel.Error, "Could not validate client certificate, client address = {ClientIp}")]
    internal static partial void CouldNotValidateCertificate(
        this ILogger<MutualTLS> logger,
        Exception exception,
        string clientIp);

    [LoggerMessage(3, LogLevel.Information, "Mutual TLS/mTLS client certificate validation is not enabled, skipping validation")]
    internal static partial void ClientCertificateValidationNotEnabled(this ILogger<MutualTLS> logger);

    [LoggerMessage(
        4,
        LogLevel.Error,
        "Client certificate failed validation. SN={SerialNumber}, IssuedBy={IssuedBy}, client address = {ClientIp}. ChainElements: {CertificateChainElements}")]
    internal static partial void InvalidCertificate(
        this ILogger<MutualTLS> logger,
        string serialNumber,
        string issuedBy,
        List<string> certificateChainElements,
        string clientIp);

    [LoggerMessage(
        5,
        LogLevel.Error,
        "Client certificate does not have an approved serial number. SN={SerialNumber}, IssuedBy={IssuedBy}, client address = {ClientIp}")]
    internal static partial void InvalidCertificateSerialNumber(
        this ILogger<MutualTLS> logger,
        string serialNumber,
        string issuedBy,
        string clientIp);

    [LoggerMessage(
        6,
        LogLevel.Information,
        "Client successfully authenticated with mTLS certificate. SN={SerialNumber}, IssuedBy={IssuedBy}, client address = {ClientIp}")]
    internal static partial void ClientLoggedIn(
        this ILogger<MutualTLS> logger,
        string serialNumber,
        string issuedBy,
        string clientIp);

    [LoggerMessage(
        7,
        LogLevel.Information,
        "Did not receive a certificate from the client, returning unauthorized. Client address = {ClientIp}")]
    internal static partial void NoCertificateReceivedFromClient(this ILogger<MutualTLS> logger, string clientIp);
}
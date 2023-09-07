// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.MutualTLS;

static partial class MutualTLSLogMessages
{
    [LoggerMessage(
        0,
        LogLevel.Error,
        "Mutual TLS/mTLS client certificate request is missing the 'x-forwarded-client-cert' header")]
    internal static partial void MissingHeader(this ILogger<MutualTLS> logger);

    [LoggerMessage(1, LogLevel.Error, "Missing certificate in the 'x-forwarded-client-cert' header")]
    internal static partial void MissingCertificateInHeader(this ILogger<MutualTLS> logger);

    [LoggerMessage(2, LogLevel.Error, "Could not validate client certificate")]
    internal static partial void CouldNotValidateCertificate(this ILogger<MutualTLS> logger, Exception exception);

    [LoggerMessage(3, LogLevel.Information, "Mutual TLS/mTLS client certificate validation is not enabled, skipping validation")]
    internal static partial void ClientCertificateValidationNotEnabled(this ILogger<MutualTLS> logger);

    [LoggerMessage(
        4,
        LogLevel.Error,
        "Client certificate failed validation. SN={SerialNumber}, IssuedBy={IssuedBy}. ChainElements: {CertificateChainElements}")]
    internal static partial void InvalidCertificate(
        this ILogger<MutualTLS> logger,
        string serialNumber,
        string issuedBy,
        List<string> certificateChainElements);

    [LoggerMessage(
        5,
        LogLevel.Error,
        "Client certificate does not have an approved serial number. SN={SerialNumber}, IssuedBy={IssuedBy}")]
    internal static partial void InvalidCertificateSerialNumber(
        this ILogger<MutualTLS> logger,
        string serialNumber,
        string issuedBy);
}
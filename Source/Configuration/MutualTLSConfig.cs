// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.IngressMiddleware.Configuration;

/// <summary>
/// Represents the configuration for mTLS/client certificates.
/// </summary>
public class MutualTLSConfig
{
    /// <summary>
    /// Gets whether or not the configuration is enabled.
    /// </summary>
    public bool IsEnabled => !string.IsNullOrEmpty(AuthorityCertificate) && (AcceptedSerialNumbers?.Any() ?? false);

    /// <summary>
    /// Gets or sets the certificate chain to validate against.
    /// To produce this value, base64 encode the appropriate cer file.
    /// For buypass virksomhetssertifikat TEST, use this file: http://crt.test4.buypass.no/crt/BPClass3T4RotCA.cer (linked here: https://community.buypass.com/t/35hltc6/test4-ca-certificate-urls).
    /// For buypass virksomhetssertifikat PROD, use this file: https://crt.buypass.no/crt/BPClass3Rot.cer (linked here: https://www.buypass.no/sikkerhet/buypass-rotsertifikater, as "Rot (Buypass Class 3 Rot CA)").
    /// </summary>
    public string AuthorityCertificate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the certificate serialnumbers to accept.
    /// </summary>
    public IList<string> AcceptedSerialNumbers { get; set; } = new List<string>();
}
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.integrationtests.mtls_auth.given;

public class factory_with_mtls_auth : Specification
{
    protected IngressWebApplicationFactory IngressFactory;
    protected HttpClient IngressClient;
    protected Config IngressConfig;
    protected X509Certificate2 CaCert;
    protected X509Certificate2 ApprovedClientCert;
    protected X509Certificate2 UnknownClientCert;

    void Establish()
    {
        // Create up a authority cert, and two client certificates for testing.
        CaCert = CreateCaCert();
        ApprovedClientCert = CreateAndSignCertificate(CaCert);
        UnknownClientCert = CreateAndSignCertificate(CaCert);

        IngressConfig = new()
        {
            TenantResolution = new()
            {
                Strategy = TenantSourceIdentifierResolverType.None
            },
            MutualTLS = new()
            {
                AuthorityCertificate = Convert.ToBase64String(CaCert.Export(X509ContentType.Pfx)),
                AcceptedSerialNumbers = new List<string>
                {
                    "serial1",
                    ApprovedClientCert.SerialNumber
                }
            }
        };

        IngressFactory = new()
        {
            Config = IngressConfig
        };

        IngressClient = IngressFactory.CreateClient();
    }

    /// <summary>
    /// Helper to set the client certificate header for the request.
    /// </summary>
    protected void SetupClientCertRequestHeader(HttpRequestMessage requestMessage, X509Certificate2 clientCert)
    {
        // Turn the client cert into the appropriate header format
        var clientBase64 = Convert.ToBase64String(clientCert.Export(X509ContentType.Pfx));
        var certHeader = $"Meh=yep;Cert=\"{HttpUtility.UrlEncode(clientBase64)}\";Chain=\"sldfkj\"";

        requestMessage.Headers.Add(Headers.ClientCertificateHeader, certHeader);
    }

    /// <summary>
    /// Creates a self-signed certificate.
    /// Stripped-down version of code found here: https://github.com/rwatjen/AzureIoTDPSCertificates/blob/master/src/DPSCertificateTool/CertificateUtil.cs.
    /// </summary>
    X509Certificate2 CreateCaCert()
    {
        using var rsa = RSA.Create();
        var request = new CertificateRequest("CN=ca_cert", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, true, 12, true));
        request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

        return request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-7), DateTimeOffset.UtcNow.AddDays(14));
    }

    /// <summary>
    /// Creates a signed client certificate.
    /// Stripped-down version of code found here: https://github.com/rwatjen/AzureIoTDPSCertificates/blob/master/src/DPSCertificateTool/CertificateUtil.cs.
    /// </summary>
    /// <param name="caCert">The certificate from <see cref="CreateCaCert"/>.</param>
    X509Certificate2 CreateAndSignCertificate(X509Certificate2 caCert)
    {
        using var rsa = RSA.Create();
        var request = new CertificateRequest("CN=client_cert", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        // set the AuthorityKeyIdentifier. There is no built-in support,
        // so it needs to be copied from the Subject Key Identifier of the signing certificate and massaged slightly.
        // AuthorityKeyIdentifier is "KeyID=<subject key identifier>"
        var issuerSubjectKey = caCert.Extensions["2.5.29.14"]!.RawData;
        var segment = new ArraySegment<byte>(issuerSubjectKey, 2, issuerSubjectKey.Length - 2);
        var authorityKeyIdentifer = new byte[segment.Count + 4];

        // these bytes define the "KeyID" part of the AuthorityKeyIdentifer
        authorityKeyIdentifer[0] = 0x30;
        authorityKeyIdentifer[1] = 0x16;
        authorityKeyIdentifer[2] = 0x80;
        authorityKeyIdentifer[3] = 0x14;
        segment.CopyTo(authorityKeyIdentifer, 4);
        request.CertificateExtensions.Add(new("2.5.29.35", authorityKeyIdentifer, false));

        var notbefore = DateTimeOffset.UtcNow.AddDays(-1);
        var notafter = DateTimeOffset.UtcNow.AddDays(1);

        var serial = Enumerable.Range(0, 20).Select(_ => (byte)Random.Shared.Next(0, 9)).ToArray();
        using var cert = request.Create(caCert, notbefore, notafter, serial);
        return cert.CopyWithPrivateKey(rsa);
    }
}
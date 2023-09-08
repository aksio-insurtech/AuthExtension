// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web;
using Aksio.IngressMiddleware.Configuration;

namespace Aksio.IngressMiddleware.MutualTLS;

/// <summary>
/// Represents an implementation of <see cref="IMutualTLS"/>.
/// </summary>
public class MutualTLS : IMutualTLS
{
    readonly Config _config;
    readonly ILogger<MutualTLS> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MutualTLS"/> class.
    /// </summary>
    /// <param name="config"><see cref="Config"/> instance.</param>
    /// <param name="logger">Logger for logging.</param>
    public MutualTLS(Config config, ILogger<MutualTLS> logger)
    {
        _config = config;
        _logger = logger;
    }

    /// <inheritdoc/>
    public bool IsEnabled() => _config.MutualTLS.IsEnabled;

    /// <inheritdoc/>
    public IActionResult Handle(HttpRequest request)
    {
        if (!_config.MutualTLS.IsEnabled)
        {
            _logger.ClientCertificateValidationNotEnabled();
            return new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }

        // Get caller address, for logging purposes.
        var clientIp = request.Headers["X-Forwarded-For"].FirstOrDefault() ?? "(n/a)";

        try
        {
            using var clientCert = GetClientCert(request, clientIp);
            if (clientCert == null)
            {
                _logger.NoCertificateReceivedFromClient(clientIp);
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            if (!_config.MutualTLS.AcceptedSerialNumbers.Any(
                    accepted => string.Equals(clientCert.SerialNumber, accepted, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.InvalidCertificateSerialNumber(clientCert.SerialNumber, clientCert.Issuer, clientIp);
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            if (!ValidateClientCert(clientCert, out var chainElements))
            {
                _logger.InvalidCertificate(clientCert.SerialNumber, clientCert.Issuer, chainElements, clientIp);
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            _logger.ClientLoggedIn(clientCert.SerialNumber, clientCert.Issuer, clientIp);
            return new OkResult();
        }
        catch (Exception ex)
        {
            _logger.CouldNotValidateCertificate(ex, clientIp);
            return new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }
    }

    /// <summary>
    /// Validate the certificate against the configured CA root.
    /// </summary>
    /// <param name="clientCert">Cert as sent by client.</param>
    /// <param name="chainElements">This list will have chain elements details.</param>
    /// <returns>True if the cert chain is valid.</returns>
    bool ValidateClientCert(X509Certificate2 clientCert, out List<string> chainElements)
    {
        chainElements = new();
        var rootCerData = Convert.FromBase64String(_config.MutualTLS.AuthorityCertificate.Trim());
        var rootCer = new X509Certificate2(rootCerData);

        using var chain = new X509Chain();
        chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
        chain.ChainPolicy.CustomTrustStore.Add(rootCer);
        chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
        var valid = chain.Build(clientCert);

        foreach (var element in chain.ChainElements)
        {
            chainElements.Add(
                $"Thumbprint: {element.Certificate.Thumbprint}, Subject: {element.Certificate.Subject}, Expiry {element.Certificate.GetExpirationDateString()}");
        }

        return valid;
    }

    /// <summary>
    /// Get the client cert sent from the ingress, and return it in X509Certificate2 format.
    /// </summary>
    /// <param name="request">The http request object.</param>
    /// <param name="clientIp">Content of X-Forwarded-For header.</param>
    /// <returns>The certificate, or null if it cannot be found.</returns>
    X509Certificate2? GetClientCert(HttpRequest request, string clientIp)
    {
        if (!request.Headers.TryGetValue(Headers.ClientCertificateHeader, out var headers))
        {
            _logger.MissingHeader(clientIp);
            return null;
        }

        var clientCertificateHeader = headers.SingleOrDefault();
        if (string.IsNullOrWhiteSpace(clientCertificateHeader))
        {
            _logger.MissingCertificateInHeader(clientIp);
            return null;
        }

        // Ingress sends us a header in the format Hash=something;Cert="urlencoded_cert_data";Chain="urlencoded_cert_data", so just extract Cert.
        var parts = clientCertificateHeader.Split(";")
            .ToDictionary(
                item => item.Split("=")[0],
                item => string.Join("=", item.Split("=").Skip(1)).TrimStart('"').TrimEnd('"'));
        var certificateData = HttpUtility.UrlDecode(parts["Cert"]);

        // Code inspired by https://github.com/stewartadam/dotnet-x509-certificate-verification/tree/main.
        certificateData = Regex.Replace(
            certificateData,
            "-+BEGIN CERTIFICATE-+",
            string.Empty,
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(1));
        certificateData = Regex.Replace(
            certificateData,
            "-+END CERTIFICATE-+",
            string.Empty,
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(1));

        return new X509Certificate2(Convert.FromBase64String(certificateData.Trim()));
    }
}
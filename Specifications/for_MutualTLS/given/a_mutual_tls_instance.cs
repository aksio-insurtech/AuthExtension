// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.IngressMiddleware.Configuration;
using MELT;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Aksio.IngressMiddleware.for_MutualTLS.given;

public class a_mutual_tls_instance : Specification
{
    /// <summary>
    /// Base64 version of buypass test CA cert for virksomhetssertifikat.
    /// </summary>
    protected string AuthorityCertificate =
        "MIIFZTCCA02gAwIBAgIBAjANBgkqhkiG9w0BAQsFADBUMQswCQYDVQQGEwJOTzEdMBsGA1UECgwUQnV5cGFzcyBBUy05ODMxNjMzMjcxJjAkBgNVBAMMHUJ1eXBhc3MgQ2xhc3MgMyBUZXN0NCBSb290IENBMB4XDTEwMTAwNjIzMDAwMFoXDTQwMTAwNjIzMDAwMFowVDELMAkGA1UEBhMCTk8xHTAbBgNVBAoMFEJ1eXBhc3MgQVMtOTgzMTYzMzI3MSYwJAYDVQQDDB1CdXlwYXNzIENsYXNzIDMgVGVzdDQgUm9vdCBDQTCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBAJgn6Yw/uPsQd3hXph48JqWO59Dui15s2J0udgvSnaAx8yIv6GKZlDnYVmUbmeJQxkcnYFoAGessqQfSBi3Wu+JANVHwPnuEGCPTNATGrYTw6Dq6WE0dlNfFlgBgtcT05yCRtQDHchFde1G9wl7mWIdTVx74gIr2ut+YaHd+XJYEvesrETYcp+BA7N8JFCW6I3CSPoZ7NRRxd996on+Vd+Knf37lR3Y+FzSAV6SvjY7jvdlZsMopYxXm3U6noVvc54Q+WO94rxoe8pt7ou/siwBH+fHzJ7JMIVNZqP/cJWe6oCEKlh+Od4ctiC9kNKdISE2j7EbhLyHT9SzUOft9oJTnC0S7oWsPMCYhEUpXfVTbs7vWRRF8pS/Kpn8kHGze/RFn/rkKIFOlmr/at4nRwB2jgXeINKHNULz2bZeSJ/7wCglXIOxgsGeqyYEsILnUsYqV1y9AKWsZdKS+GlGRsHibUYpr739+qN86YayQlMLFD+bVndH200NVoy5f17SuVqtG231VjV2mGIOwPRpq5eLPHqPcNhujkOmXS1HMDJJOL1gOGwvINy1SJU7RTCDA0oc0jNKIqOjJLyiTUwO+kzu6nbY3LqaxhwgCrZGVado0DQWE+wHu0g2O7QwiuNRX5wkraWwPAc5Q0wSaBl/yx158V9hh6GPtjM7QRc/pMn5DAgMBAAGjQjBAMA8GA1UdEwEB/wQFMAMBAf8wHQYDVR0OBBYEFO20zz97Ixh2OsatsmsAXXrGiNbrMA4GA1UdDwEB/wQEAwIBBjANBgkqhkiG9w0BAQsFAAOCAgEAKMKFZqTVp9hEkqUfQZvxekzD1hGYSf77aj8lwVk/5E1B2lHVotTzkq7kuvFLtFxU6qR6H1Pn5o0BuEVi9S7RpD3TNXQPYVda2ZBOrYgGUKD7NzNPyWW+vDobOKNlLUfwsnjuCXKQSo/t57z5FYLPfOx1gmluIehc2ml3Wf9R+0WTjTSMn0k3Hzy/CqidUJ2vpm/5u5x0wXxrIY3Kg+9IdoWj1aesbLL5sOdvgKDzczDqDmt1uY9CDkXuPD/qGtW0AdVIftmnxb6JTn2wQ6JSMrGnlY6BsUujqNPoeVqy8udQ+ckUgzJzhZ6jpRHqq7aP8JbpVx3KZw7LmSdMrwSZaZZfMa9OK2G1Kb9XPVKPB5/KWS9de+JJEXVjqgK/2VMU66y/PRZqV2C83xTSZEE47P3b8EU9qyM7Kii/e8UEGRQR2Q2i2tRp1yHBzwekKZgtYzA0poGVj7NWnCAm0OlCWyBP47MyiMS4npqNYf8wMhRy4fTY+yy5ORJp5Fc08/VbTt62CPpwqh06S7H05Z+IzRroG9yp0NBy6Z17/db3JnosbfnHdyFlZqze2ILjMdBwM/edPQgGqhpgn2UDNTE8SmVLDmnq0hGBR9sAA0XedFjA0Cvxi7AlX4Bb9MuAQzrfTcFikvJmvmUY4+935G70lrW1oSXZugFqgAM8FVJP1pY=";

    /// <summary>
    /// Public component of aksio test virksomhetssertifikat, as sent by Azure Container App ingress.
    /// </summary>
    protected string ClientCertificate =
        "Hash=123123;Cert=\"-----BEGIN%20CERTIFICATE-----%0AMIIFJzCCBA%2BgAwIBAgILBPVdHbFdWXwi1ZAwDQYJKoZIhvcNAQELBQAwUTELMAkG%0AA1UEBhMCTk8xHTAbBgNVBAoMFEJ1eXBhc3MgQVMtOTgzMTYzMzI3MSMwIQYDVQQD%0ADBpCdXlwYXNzIENsYXNzIDMgVGVzdDQgQ0EgMzAeFw0yMTEwMTIxNzMyNTFaFw0y%0ANDEwMTIyMTU5MDBaMG4xCzAJBgNVBAYTAk5PMRgwFgYDVQQKDA9BS1NJTyBTWVNU%0ARU0gQVMxEjAQBgNVBAsMCVV0dmlrbGluZzEdMBsGA1UEAwwUQWtzaW8gU3lzdGVt%0AIEFTIFRFU1QxEjAQBgNVBAUTCTkyNzYxMzI5ODCCASIwDQYJKoZIhvcNAQEBBQAD%0AggEPADCCAQoCggEBAJ0RlezRiMzbGGisIZ2nmWah9LwoYZAMqEtyjHwvTW%2B2NWC2%0AinCX8U6waQNjl%2BDhDxOeO%2FS%2F6QYXbqIM7C5mQDlZqJSBj8%2F21YEuHNV3GM%2Biyvjq%0AplZ7cgyonPs%2FfupJ1Q9OQpXWT0gEBUIyuqGUu0XBpU%2BsC6t%2FKz7WWcxQ%2BUqwY2le%0AnBKZxlix1SE3SBK3QfZ0w8JC%2B00ZHnneidhbYnYanKmhtFR0XEOnDIf9GRn%2BpGiQ%0A4Vw4rhyfkOnQpTKM29RLEb0fbKEwLWWgyo6GgN0yqaKfBRzDmuisskQLx82vtj%2Bv%0ALSVRZjV40zDuvPuVbvyG4qu8KJBSynmBXGXwWLkCAwEAAaOCAeEwggHdMAkGA1Ud%0AEwQCMAAwHwYDVR0jBBgwFoAUP671eAuSo3AgNV9a%2BvckoFIB8EEwHQYDVR0OBBYE%0AFDN1AM4Bq2zHfYImv3u61KlMR2DQMA4GA1UdDwEB%2FwQEAwIEsDAdBgNVHSUEFjAU%0ABggrBgEFBQcDAgYIKwYBBQUHAwQwFgYDVR0gBA8wDTALBglghEIBGgEAAwIwgbsG%0AA1UdHwSBszCBsDA3oDWgM4YxaHR0cDovL2NybC50ZXN0NC5idXlwYXNzLm5vL2Ny%0AbC9CUENsYXNzM1Q0Q0EzLmNybDB1oHOgcYZvbGRhcDovL2xkYXAudGVzdDQuYnV5%0AcGFzcy5uby9kYz1CdXlwYXNzLGRjPU5PLENOPUJ1eXBhc3MlMjBDbGFzcyUyMDMl%0AMjBUZXN0NCUyMENBJTIwMz9jZXJ0aWZpY2F0ZVJldm9jYXRpb25MaXN0MIGKBggr%0ABgEFBQcBAQR%2BMHwwOwYIKwYBBQUHMAGGL2h0dHA6Ly9vY3NwLnRlc3Q0LmJ1eXBh%0Ac3Mubm8vb2NzcC9CUENsYXNzM1Q0Q0EzMD0GCCsGAQUFBzAChjFodHRwOi8vY3J0%0ALnRlc3Q0LmJ1eXBhc3Mubm8vY3J0L0JQQ2xhc3MzVDRDQTMuY2VyMA0GCSqGSIb3%0ADQEBCwUAA4IBAQCtzLzVPTAriHjsCa%2BTXjf5sufooopzP3pW7%2Fune41RwNUV3AmY%0AiA%2BF5znvqHNyuV%2FW5kwbhAML3hDXH1%2BcpeqhCl5uzDV1EqJwXonTxQi7XK29KEmi%0AL4ZARQmvRjLylqu9LwhDYperl4f5tVbBw7WRv%2BtzWo%2FikvZB8zdrnpJ0tblW4keK%0ANOnz26QbhezaxaworncuZ%2Fe6AU%2BqhKuAWc1zEcNb0qKS8CqUnDCrkwckcYfGqVOJ%0A8kEQMRH8mBs3NNUrXU2w9dhqvjPOXNp%2FW8ZzCkVTGYbQsf1efdM7vQfpjxsYPoWD%0APHtgQ5laT7BK4GgEVBB1CSq3VyrSiAY3l58G%0A-----END%20CERTIFICATE-----%0A\"";

    protected string ClientCertificateSerial = "04f55d1db15d597c22d590";

    protected MutualTLS.MutualTLS MutualTls;
    protected HttpRequest Request;

    protected MutualTLSConfig MutualTlsConfig;
    protected Config Config;

    protected ITestLoggerFactory LoggerFactory;
    protected ILogger<MutualTLS.MutualTLS> Logger;

    void Establish()
    {
        Request = new DefaultHttpContext().Request;
        Request.Headers[Headers.ClientCertificateHeader] = ClientCertificate;

        // https://alessio.franceschelli.me/posts/dotnet/how-to-test-logging-when-using-microsoft-extensions-logging/
        LoggerFactory = TestLoggerFactory.Create();
        Logger = LoggerFactory.CreateLogger<MutualTLS.MutualTLS>();

        MutualTlsConfig = new()
        {
            AuthorityCertificate = AuthorityCertificate,
            AcceptedSerialNumbers = new List<string> { ClientCertificateSerial }
        };

        Config = new()
        {
            MutualTLS = MutualTlsConfig
        };

        MutualTls = new(Config, Logger);
    }
}
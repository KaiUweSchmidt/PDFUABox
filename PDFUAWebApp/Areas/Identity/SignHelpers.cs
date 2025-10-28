using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PDFUABox.WebApp.Areas.Identity;

internal static class SignHelpers
{

    public static string CreateCertificate(string userName, string password)
    {
        var distinguishedName = new X500DistinguishedName($"CN={userName}");
        using RSA rsa = RSA.Create(2048);
        var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        // Add basic extensions
        request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
        request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, false));
        request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

        // Create the certificate
        var certificate = request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(30));

        // Export and save as PFX
        byte[] pfxBytes = certificate.Export(X509ContentType.Pfx, password);
        return Convert.ToBase64String(pfxBytes);
    }
}

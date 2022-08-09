using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Security.Cryptography;

namespace TURI.Contractservice.Tests.Integration.Helpers
{
    internal static class Jwt256Keys
    {
        static Jwt256Keys()
        {
            // Unlock PII for clearer error messages while under test
            IdentityModelEventSource.ShowPII = true;
        }

        /// <summary>
        /// Gets the private <see cref="RsaSecurityKey"/> configured by the test
        /// private key (PEM) file in <c>configs/private_key.pem</c>.
        /// </summary>
        public static RsaSecurityKey PrivateSecurityKey
        {
            get
            {
                var key = File.ReadAllText("configs/private_key.pem")
                    .Replace("BEGIN PRIVATE KEY", "")
                    .Replace("END PRIVATE KEY", "")
                    .Replace("\r\n", "")
                    .Replace(" ", "")
                    .Replace("-", "");

                var rsa = RSA.Create();
                rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(key), out _);
                return new RsaSecurityKey(rsa);
            }
        }

        /// <summary>
        /// Gets the public <see cref="RsaSecurityKey"/> configured by the test
        /// public key file in <c>configs/jwtRS246.key.pub</c>.
        /// </summary>
        public static RsaSecurityKey PublicSecurityKey
        {
            get
            {
                var key = File.ReadAllText("configs/jwtRS256.key.pub")
                    .Replace("BEGIN PUBLIC KEY", "")
                    .Replace("END PUBLIC KEY", "")
                    .Replace("\r\n", "")
                    .Replace(" ", "")
                    .Replace("-", "");

                var rsa = RSA.Create();
                rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(key), out _);
                return new RsaSecurityKey(rsa);
            }
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace TURI.Contractservice.Api.StartupExtensions
{
    internal static class JwtBearerStartupExtensions
    {
        /// <summary>
        /// Configures the <see cref="JwtBearerOptions"/> for federated authentication with JWT
        /// using a RSA 256 public key file in the file system.
        /// </summary>
        /// <param name="options">The current <see cref="JwtBearerOptions"/>.</param>
        /// <param name="keyFilePath">The path to the RSA public key file.</param>
        public static void ConfigureWithPublicKeyFile(this JwtBearerOptions options, string keyFilePath)
        {
            var cleanPublicKey = File.ReadAllText(keyFilePath)
                   .Replace("BEGIN PUBLIC KEY", "")
                   .Replace("END PUBLIC KEY", "")
                   .Replace("\r\n", "")
                   .Replace(" ", "")
                   .Replace("-", "");

            byte[] publicKeyBytes = Convert.FromBase64String(cleanPublicKey);

            var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

            options.TokenValidationParameters.IssuerSigningKey = new RsaSecurityKey(rsa);
        }

        /// <summary>
        /// When multiple identities conform the same Principal, this setup ensures that the
        /// <see cref="ClaimsPrincipal.Identity"/> is set up to the one provided by the selected
        /// scheme. When multiple schemes are added in configuration, these are evaluated
        /// sequentially and the first one that finds a match is used as the default identity. If no
        /// matches are found, the default behaviour of using any available identity is used.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
        /// <param name="authenticationTypes">
        /// The authentication types in the order of priority for the selector.
        /// </param>
        public static AuthenticationBuilder SetupPrimaryAuthenticationTypes(this AuthenticationBuilder builder, params string[] authenticationTypes)
        {
            ClaimsPrincipal.PrimaryIdentitySelector = identities =>
            {
                foreach (var authenticationType in authenticationTypes)
                {
                    var id = identities.FirstOrDefault(x => x.AuthenticationType == authenticationType);
                    if (id != null) return id;
                }
                return identities.FirstOrDefault();
            };

            return builder;
        }
    }
}
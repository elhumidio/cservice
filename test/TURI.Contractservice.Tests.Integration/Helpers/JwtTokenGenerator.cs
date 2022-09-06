using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace TURI.Contractservice.Tests.Integration.Helpers
{
    internal static class JwtTokenGenerator
    {
        /// <summary>
        /// Generates a new JWT token string.
        /// </summary>
        private static string GenerateToken(SecurityTokenDescriptor descriptor)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.CreateEncodedJwt(descriptor);
        }

        public static string GenerateStepStoneDEToken()
        {
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "stepstone.de",
                Expires = DateTime.UtcNow.AddDays(7),
                Claims = new Dictionary<string, object>
                {
                    { "sub", 1234567890 },
                    { "type", "cli" },
                    { "fully_logged_in", true },
                },
                SigningCredentials = new SigningCredentials(Jwt256Keys.PrivateSecurityKey, SecurityAlgorithms.RsaSha256)
            };

            return GenerateToken(descriptor);
        }
    }
}

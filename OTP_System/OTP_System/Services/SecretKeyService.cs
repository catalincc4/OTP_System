using System.Security.Cryptography;
using OTP_System.Interfaces;

namespace OTP_System.Services
{
    public class SecretKeyService : ISecretKeyService 
    {
        public string GenerateSecretKey()
        {

            var secretKeyBytes = RandomNumberGenerator.GetBytes(32);

            string secretKey = Convert.ToBase64String(secretKeyBytes);

            return secretKey;
        }
    }
}

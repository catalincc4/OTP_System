using OTP_System.Models;
using OTP_System.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace OTP_System.Services
{
    public class OtpGeneratorService : IOtpGeneratorService
    {
        private long GetCurrentCounter(long timeStep)
        {
            TimeSpan elapsedTime = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return (long)(elapsedTime.TotalSeconds / timeStep);
        }

        private byte[] ConvertSecretKeyToBytes(string secretKey)
        {
            return Encoding.UTF8.GetBytes(secretKey);
        }

        private string GenerateOtp(string secretKey, long counter, int digitLength)
        {
            byte[] counterBytes = BitConverter.GetBytes(counter);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(counterBytes);
            }

            var secretKeyBytes = ConvertSecretKeyToBytes(secretKey);

            HMACSHA1 hmac = new HMACSHA1(secretKeyBytes);
            byte[] hash = hmac.ComputeHash(counterBytes);

            int offset = hash[hash.Length - 1] & 0xF;
            int binary = ((hash[offset] & 0x7F) << 24)
                         | ((hash[offset + 1] & 0xFF) << 16)
                         | ((hash[offset + 2] & 0xFF) << 8)
                         | (hash[offset + 3] & 0xFF);

            int otp = binary % (int)Math.Pow(10, digitLength);
            return otp.ToString().PadLeft(digitLength, '0');
        }

        public string GenerateOtpForUser(User user)
        {

            long counter = GetCurrentCounter(user.OtpDuration);
            return GenerateOtp(user.SecretKey, counter, user.OtpLenght);

        }

        public bool VerifyOtp(User user, string userEnteredCode)
        {
            long currentCounter = GetCurrentCounter(user.OtpDuration);

            for (int i = -1; i <= 1; i++)
            {
                long counter = currentCounter + i;
                string otp = GenerateOtp(user.SecretKey, counter, user.OtpLenght);
                if (otp == userEnteredCode)
                {
                    return true;
                }
            }

            return false;
        }

    }
}

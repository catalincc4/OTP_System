using OTP_System.Interfaces;
using OTP_System.Models;
using System.Security.Cryptography;
using System.Text;

namespace OTP_System.Services
{
    public class OtpService : IOtpService
    {

        private readonly IOtpRepository _otpRepository;
        public OtpService(IOtpRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }

        private long GetCurrentCounter()
        {
            TimeSpan elapsedTime = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return (long)(elapsedTime.TotalSeconds / 30);
        }

        private byte[] ConvertSecretKeyToBytes(string secretKey)
        {
            return Encoding.UTF8.GetBytes(secretKey);
        }

        private string GenerateOtp(string secretKey, long counter)
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

            int otp = binary % (int)Math.Pow(10, 6);
            return otp.ToString().PadLeft(6, '0');
        }

        public string GenerateOtpForUser(User user)
        {
            long counter = GetCurrentCounter();
            return GenerateOtp(user.SecretKey, counter);

        }

        public bool VerifyOtp(User user, string userEnteredCode)
        {
            long currentCounter = GetCurrentCounter();

            var usedOtp = _otpRepository.FindByUserIdAndOtpCode(user.Id, userEnteredCode);
            if (usedOtp != null && currentCounter - usedOtp.Counter <= 1) { 
                return false;
            }


            for (int i = -1; i <= 1; i++)
            {
                long counter = currentCounter + i;
                string otp = GenerateOtp(user.SecretKey, counter);
                if (otp == userEnteredCode)
                {

                    var newOtp = new Otp
                    {
                        UserId = user.Id,
                        OtpCode = otp,
                        Counter = counter,
                        UsedAt = DateTime.UtcNow,
                        User = user
                    };

                    _otpRepository.Add(newOtp);

                    return true;
                }
            }

            return false;
        }
    }
}

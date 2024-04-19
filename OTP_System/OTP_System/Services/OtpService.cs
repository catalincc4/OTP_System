using Azure.Core;
using Microsoft.EntityFrameworkCore;
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

        public string GenerateOtpForUser(User user, long validaionTime)
        {
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();

            var otpEntity = new Otp
            {
                UserId = user.Id,
                OtpCode = otp,
                validationTime = validaionTime, 
                ExpiryTime = DateTime.Now.Add(TimeSpan.FromSeconds(validaionTime))
            };
            _otpRepository.Add(otpEntity);

            return otp;
        }

        public bool VerifyOtp(User user, string userEnterdeCode)
        {
            string userId = user.Id;

            var otpEntity = _otpRepository.FindByUserIdAndOtpCode(userId, userEnterdeCode);

            if (otpEntity != null && otpEntity.ExpiryTime >= DateTime.Now)
            {
                _otpRepository.Remove(otpEntity);
                return true;
            }

            return false;
        }

    }
}

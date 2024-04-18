using OTP_System.Models;

namespace OTP_System.Services.Interfaces
{
    public interface IOtpGeneratorService
    {
        public string GenerateOtpForUser(User user);
        public bool VerifyOtp(User user, string userEnterdeCode);
    }
}
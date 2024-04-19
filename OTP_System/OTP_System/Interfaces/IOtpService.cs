using OTP_System.Models;

namespace OTP_System.Interfaces
{
    public interface IOtpService
    {
        public string GenerateOtpForUser(User user, long validationTime);
        public bool VerifyOtp(User user, string userEnterdeCode);
    }
}
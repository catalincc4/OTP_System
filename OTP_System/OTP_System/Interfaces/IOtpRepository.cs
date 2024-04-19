using Microsoft.EntityFrameworkCore.ChangeTracking;
using OTP_System.Models;

namespace OTP_System.Interfaces
{
    public interface IOtpRepository
    {
        Otp FindByUserIdAndOtpCode(string userId, string otpCode);

        EntityEntry<Otp> Add(Otp otp);

        void Remove(Otp otp);
    }
}

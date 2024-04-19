using Microsoft.EntityFrameworkCore.ChangeTracking;
using OTP_System.Data;
using OTP_System.Interfaces;
using OTP_System.Models;

namespace OTP_System.Repositories
{
    public class OtpRepository : IOtpRepository
    {
        private readonly DataContext _dataContext;
        public OtpRepository(DataContext dataContext) { 
            _dataContext = dataContext;
        }

        public  Otp FindByUserIdAndOtpCode(string userId, string otpCode)
        {
            var otp = _dataContext.Otps
                .Where(o => o.UserId == userId && o.OtpCode == otpCode)
                .OrderByDescending(o => o.UsedAt)
                .FirstOrDefault();

            return otp;
        }

        public EntityEntry<Otp> Add(Otp otp)
        {
            var entry = _dataContext.Add(otp);
            _dataContext.SaveChanges();
            return entry;
        }
    }
}

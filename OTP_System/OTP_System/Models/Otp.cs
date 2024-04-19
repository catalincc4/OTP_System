using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OTP_System.Models
{
    public class Otp
    {
        [Required]
        public int Id { get; set; }
        public string OtpCode { get; set; }
        public long validationTime {  get; set; }
        public DateTime ExpiryTime { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}

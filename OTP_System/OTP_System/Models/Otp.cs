using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OTP_System.Models
{
    public class Otp
    {
        [Required]
        public int ID { get; set; }
        public string OtpCode { get; set; }
        public long Counter { get; set; }
        public DateTime UsedAt { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}

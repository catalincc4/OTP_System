using System.ComponentModel.DataAnnotations;

namespace OTP_System.dtos
{
    public class SendOtpDto
    {
        [Required]
        public string UserEmail { get; set; }
    }
}

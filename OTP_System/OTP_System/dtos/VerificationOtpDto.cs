using System.ComponentModel.DataAnnotations;
using System.Security;

namespace OTP_System.dtos
{
    public class VerificationOtpDto
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string UserEnteredCode {  get; set; }
    }
}

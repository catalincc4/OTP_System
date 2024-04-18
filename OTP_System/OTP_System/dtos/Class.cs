using System.ComponentModel.DataAnnotations;
using System.Security;

namespace OTP_System.dtos
{
    public class VerifcationOtpDto
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string UserenterdeCode {  get; set; }
    }
}

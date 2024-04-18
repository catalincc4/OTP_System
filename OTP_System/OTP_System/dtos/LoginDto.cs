using System.ComponentModel.DataAnnotations;

namespace OTP_System.dtos
{
    public class LoginDto
    {
        [Required]
        public  string Email {  get; set; }
        [Required]
        [MinLength(8)]
        public  string Password { get; set; }
    }
}

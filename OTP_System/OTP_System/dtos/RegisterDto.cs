using System.ComponentModel.DataAnnotations;

namespace OTP_System.dtos
{
    public class RegisterDto
    {
        [Required]
        public  string Email { get; set; }
        [Required]
        [MinLength(8)]
        public  string Password { get; set; }
        [Required]
        public  string FirstName { get; set; }
        [Required]
        public  string LastName { get; set; }
    }
}

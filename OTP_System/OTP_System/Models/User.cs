using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OTP_System.Models
{
    public class User : IdentityUser
    {

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;

        public int OtpLenght { get; set; } = 6;
        public int OtpDuration { get; set; } = 30;

    }
}

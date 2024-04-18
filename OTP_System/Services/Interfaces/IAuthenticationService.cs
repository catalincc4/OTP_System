using OTP_System.dtos;
using Microsoft.AspNetCore.Identity;
using OTP_System.Models;
using System.IdentityModel.Tokens.Jwt;

namespace OTP_System.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<IdentityResult> RegisterAsync(RegisterDto model);
        public Task<User> ValidateCredentialsAsync(LoginDto model);

        public Task<bool> IsTwoFactorAuthenticationEnabledAsync(User user);

        public JwtSecurityToken GenerateTokenAsync(User user);
    }
}

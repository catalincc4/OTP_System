using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OTP_System.dtos;
using OTP_System.Interfaces;
using OTP_System.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OTP_System.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly ISecretKeyService _secretKeyService;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<User> userManager, 
            SignInManager<User> signInManager,
            ILogger<AuthenticationService> logger,
            ISecretKeyService secretKeyService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _secretKeyService = secretKeyService;
            _configuration = configuration;
        }

        public JwtSecurityToken GenerateTokenAsync(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("email", user.Email)
            };

            var token = GetToken(authClaims);
            return token;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public async Task<bool> IsTwoFactorAuthenticationEnabledAsync(User user)
        {
            return await _signInManager.IsTwoFactorEnabledAsync(user);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto model)
        {
            var secretKey = _secretKeyService.GenerateSecretKey();
            var user = new User {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                SecretKey = secretKey
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            return result;
        }

        public async Task<User> ValidateCredentialsAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null || !await _userManager.CheckPasswordAsync(user, model.Password)) {
                return null;
            }

            return user;

        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OTP_System.dtos;
using OTP_System.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace OTP_System.Controllers
{
    [Route("")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authenticationService.RegisterAsync(model);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Registration successful." });
            }

            return BadRequest(new { result.Errors });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _authenticationService.ValidateCredentialsAsync(model);
            if (user != null)
            {
                if (await _authenticationService.IsTwoFactorAuthenticationEnabledAsync(user)) {
                    return Ok(new { UserId = user.Id });
                }

                var token = _authenticationService.GenerateTokenAsync(user);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized("Invalid email or password");
           
        }
    }
}

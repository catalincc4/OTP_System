using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTP_System.dtos;
using OTP_System.Interfaces;
using OTP_System.Models;

namespace OTP_System.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IOtpService _otpService;

        public OtpController(UserManager<User> userManager, IOtpService otpService)
        {
            _userManager = userManager;
            _otpService = otpService;
        }

        [HttpPost("/sendOtp")]
        public async Task<IActionResult> SendOtpForUser(SendOtpDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Provide an email");
            }
            var user = await _userManager.FindByEmailAsync(model.UserEmail);

            if (user == null)
            {
                return BadRequest("Inccorect email");
            }
            var otp = _otpService.GenerateOtpForUser(user, model.ValidationTime);
            return Ok(new { otp = otp});
        }

        [HttpPost("/verifyOtp")]
        public async Task<IActionResult> VerifyOtp(VerificationOtpDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Incorrect information provided");
            }
            var user = await _userManager.FindByEmailAsync(model.UserEmail);

            if (user == null)
            {
                return BadRequest("Inccorect email");
            }

            var otpValid = _otpService.VerifyOtp(user, model.UserEnteredCode);

            if (!otpValid)
            {
                return BadRequest("The code entered is invalid");
            }
            return Ok(new { message = "Validation successfully" });

        }
      
    }
}

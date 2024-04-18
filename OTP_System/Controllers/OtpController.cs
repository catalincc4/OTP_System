using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTP_System.dtos;
using OTP_System.Models;
using OTP_System.Services.Interfaces;

namespace OTP_System.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IOtpGeneratorService _otpGeneratorService;
        public OtpController(UserManager<User> userManager, IOtpGeneratorService otpGeneratorService)
        {
            _userManager = userManager;
            _otpGeneratorService = otpGeneratorService;
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
            var otp = _otpGeneratorService.GenerateOtpForUser(user);
            return Ok(otp);
        }

        [HttpPost("/verifyOtp")]
        public async Task<IActionResult> VerifyOtp(VerifcationOtpDto model)
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

            var otpValid = _otpGeneratorService.VerifyOtp(user, model.UserenterdeCode);

            if (!otpValid)
            {
                return BadRequest("The code entered is invalid");
            }
            return Ok("validation succesfull");

        }
      
    }
}

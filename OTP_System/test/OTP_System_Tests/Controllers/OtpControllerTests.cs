using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AutoFixture;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTP_System.Controllers;
using OTP_System.dtos;
using OTP_System.Interfaces;
using OTP_System.Models;

namespace OTP_System_Tests.Controllers
{
    public class OtpControllerTests
    {
        private readonly OtpController _controller;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IOtpService> _otpServiceMock;

        public OtpControllerTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _otpServiceMock = new Mock<IOtpService>();
            _controller = new OtpController(_userManagerMock.Object, _otpServiceMock.Object);
        }

        [Fact]
        public async Task SendOtpForUser_ReturnsBadRequest_WhenModelInvalid()
        {
            
            _controller.ModelState.AddModelError("UserEmail", "The UserEmail field is required");
            var model = new SendOtpDto();

          
            var result = await _controller.SendOtpForUser(model);

            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Provide an email");
        }

        [Fact]
        public async Task SendOtpForUser_ReturnsBadRequest_WhenUserNotFound()
        {
          
            var model = new SendOtpDto { UserEmail = "nonexistent@example.com" };
            _userManagerMock.Setup(x => x.FindByEmailAsync(model.UserEmail)).ReturnsAsync((User)null);
 
            var result = await _controller.SendOtpForUser(model);

            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Inccorect email");

            _userManagerMock.Verify(x => x.FindByEmailAsync(model.UserEmail), Times.Once());
        }

        [Fact]
        public async Task SendOtpForUser_ReturnsOk_WhenOtpGenerated()
        {
            var model = new SendOtpDto { UserEmail = "existing@example.com",ValidationTime = 30};
            var user = new User();
            _userManagerMock.Setup(x => x.FindByEmailAsync(model.UserEmail)).ReturnsAsync(user);
            _otpServiceMock.Setup(x => x.GenerateOtpForUser(user, 30)).Returns("123456");

            var result = await _controller.SendOtpForUser(model);

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { otp = "123456" });

            _userManagerMock.Verify(x => x.FindByEmailAsync(model.UserEmail), Times.Once());
            _otpServiceMock.Verify(x => x.GenerateOtpForUser(user, 30), Times.Once());
        }

        [Fact]
        public async Task VerifyOtp_ReturnsBadRequest_WhenModelInvalid()
        {
            _controller.ModelState.AddModelError("UserEmail", "The UserEmail field is required");
            var model = new VerificationOtpDto();

            var result = await _controller.VerifyOtp(model);

            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Incorrect information provided");
        }

        [Fact]
        public async Task VerifyOtp_ReturnsBadRequest_WhenUserNotFound()
        {
            var model = new VerificationOtpDto { UserEmail = "nonexistent@example.com" };
            _userManagerMock.Setup(x => x.FindByEmailAsync(model.UserEmail)).ReturnsAsync((User)null);

            var result = await _controller.VerifyOtp(model);

            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Inccorect email");

            _userManagerMock.Verify(x => x.FindByEmailAsync(model.UserEmail), Times.Once());
        }

        [Fact]
        public async Task VerifyOtp_ReturnsBadRequest_WhenOtpInvalid()
        {
            var model = new VerificationOtpDto { UserEmail = "existing@example.com", UserEnteredCode = "123456" };
            var user = new User();
            _userManagerMock.Setup(x => x.FindByEmailAsync(model.UserEmail)).ReturnsAsync(user);
            _otpServiceMock.Setup(x => x.VerifyOtp(user, model.UserEnteredCode)).Returns(false);

            var result = await _controller.VerifyOtp(model);

            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("The code entered is invalid");

            _userManagerMock.Verify(x => x.FindByEmailAsync(model.UserEmail), Times.Once());
            _otpServiceMock.Verify(x => x.VerifyOtp(user, model.UserEnteredCode), Times.Once());
        }

        [Fact]
        public async Task VerifyOtp_ReturnsOk_WhenOtpValid()
        {
            var model = new VerificationOtpDto { UserEmail = "existing@example.com", UserEnteredCode = "123456" };
            var user = new User();
            _userManagerMock.Setup(x => x.FindByEmailAsync(model.UserEmail)).ReturnsAsync(user);
            _otpServiceMock.Setup(x => x.VerifyOtp(user, model.UserEnteredCode)).Returns(true);

            var result = await _controller.VerifyOtp(model);

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { message = "Validation successfully" });

            _userManagerMock.Verify(x => x.FindByEmailAsync(model.UserEmail), Times.Once());
            _otpServiceMock.Verify(x => x.VerifyOtp(user, model.UserEnteredCode), Times.Once());
        }
    }
}

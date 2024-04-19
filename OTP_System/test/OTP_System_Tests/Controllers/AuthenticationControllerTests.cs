using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AutoFixture;
using Moq;
using FluentAssertions;
using OTP_System.Interfaces;
using OTP_System.Controllers;
using Microsoft.AspNetCore.Identity;
using OTP_System.dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using OTP_System.Models;
using System.IdentityModel.Tokens.Jwt;

namespace OTP_System_Tests.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IAuthenticationService> _serviceMock;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IAuthenticationService>>();
            _controller = new AuthenticationController(_serviceMock.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenModelInvalid()
        {
            var request = _fixture.Create<RegisterDto>();
            _controller.ModelState.AddModelError("Email", "The Email field is required");

            var identityResult = _fixture.Create<IdentityResult>();
            _serviceMock.Setup(x => x.RegisterAsync(request)).ReturnsAsync(identityResult);

            var result = await _controller.Register(request);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();

        }


        [Fact]
        public async Task Register_ShouldReturnOk_WhenRegistrationSuccessful()
        {
      
            var model = new RegisterDto {
                Email = "calin1@mail.com",
                FirstName = "Test",
                LastName = "Test",
                Password = "Test12!"
            };
            var result = IdentityResult.Success;
            _serviceMock.Setup(x => x.RegisterAsync(model)).ReturnsAsync(result);

       
            var actionResult = await _controller.Register(model);

       
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeEquivalentTo(new { Message = "Registration successful." });
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenRegistrationFails()
        {
          
            var model = new RegisterDto
            {
                Email = "calin1@mail.com",
                FirstName = "Test",
                LastName = "Test",
                Password = "Test12!"
            };
            var errors = new List<IdentityError> { new IdentityError { Description = "Error message" } };
            var result = IdentityResult.Failed(errors.ToArray());
            _serviceMock.Setup(x => x.RegisterAsync(model)).ReturnsAsync(result);

            var actionResult = await _controller.Register(model);

            var badRequestResult = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
        }

        //Login


        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenModelStateInvalid()
        {
            var model = new LoginDto { Password = "aaaaa"};
            _controller.ModelState.AddModelError("Email", "The Email field is required");

            var actionResult = await _controller.Login(model);

            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenInvalidCredentials()
        {
            // Arrange
            User nullUser = null; 
            var model = new LoginDto { Email = "calin3@mail.com", Password = "catalin12C!" };
            _serviceMock.Setup(x => x.ValidateCredentialsAsync(model)).ReturnsAsync(nullUser);

            // Act
            var actionResult = await _controller.Login(model);

            // Assert
            actionResult.Should().BeOfType<UnauthorizedObjectResult>()
                .Which.Value.Should().Be("Invalid email or password");
            _serviceMock.Verify(x => x.ValidateCredentialsAsync(model), Times.Once());
        }

        [Fact]
        public async Task Login_ShouldReturnOkWithToken_WhenValidCredentialsAndNoTwoFactorAuth()
        {
            var model = new LoginDto { };
            var user = new User { Id = "userId" };
            _serviceMock.Setup(x => x.ValidateCredentialsAsync(model)).ReturnsAsync(user);
            _serviceMock.Setup(x => x.IsTwoFactorAuthenticationEnabledAsync(user)).ReturnsAsync(false);
            var token = new JwtSecurityToken();
            _serviceMock.Setup(x => x.GenerateTokenAsync(user)).Returns(token);

            var actionResult = await _controller.Login(model);

            actionResult.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });

            _serviceMock.Verify(x => x.ValidateCredentialsAsync(model), Times.Once());
            _serviceMock.Verify(x => x.IsTwoFactorAuthenticationEnabledAsync(user), Times.Once());
        }

        [Fact]
        public async Task Login_ShouldReturnOkWithUserId_WhenValidCredentialsAndTwoFactorAuthEnabled()
        {
            var model = new LoginDto { };
            var user = new User { Id = "userId" };
            _serviceMock.Setup(x => x.ValidateCredentialsAsync(model)).ReturnsAsync(user);
            _serviceMock.Setup(x => x.IsTwoFactorAuthenticationEnabledAsync(user)).ReturnsAsync(true);

            var actionResult = await _controller.Login(model);

            actionResult.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { UserId = user.Id });

            _serviceMock.Verify(x => x.ValidateCredentialsAsync(model), Times.Once());
            _serviceMock.Verify(x => x.IsTwoFactorAuthenticationEnabledAsync(user), Times.Once());

        }


    }
}

using FluentAssertions;
using Moq;
using OTP_System.Interfaces;
using OTP_System.Models;
using OTP_System.Services;

namespace OTP_System_Tests.Services
{
    public class OtpServiceTests
    {
        private readonly OtpService _otpService;
        private readonly Mock<IOtpRepository> _otpRepositoryMock;

        public OtpServiceTests()
        {
            _otpRepositoryMock = new Mock<IOtpRepository>();
            _otpService = new OtpService(_otpRepositoryMock.Object);
        }

        [Fact]
        public void GenerateOtpForUser_ShouldGenerateOtp()
        {
            var user = new User { Id = "userId"};

            var otp = _otpService.GenerateOtpForUser(user, 30);

            otp.Should().NotBeNullOrEmpty();
        }

    }
}

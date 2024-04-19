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
            var user = new User { Id = "userId", SecretKey = "secretKey" };

            var otp = _otpService.GenerateOtpForUser(user);

            otp.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void VerifyOtp_ShouldReturnTrue_WhenValidOtpEntered()
        {

            var user = new User { Id = "userId", SecretKey = "secretKey" };
            var userEnteredCode = _otpService.GenerateOtpForUser(user);
            var currentCounter = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            _otpRepositoryMock.Setup(x => x.FindByUserIdAndOtpCode(user.Id, userEnteredCode)).Returns((Otp)null);

            var result = _otpService.VerifyOtp(user, userEnteredCode);

            result.Should().BeTrue();
            _otpRepositoryMock.Verify(x => x.Add(It.IsAny<Otp>()), Times.Once());
        }

        [Fact]
        public void VerifyOtp_ShouldReturnFalse_WhenInvalidOtpEntered()
        {
            var user = new User { Id = "userId", SecretKey = "secretKey" };
            var userEnteredCode = "invalidCode";
            var currentCounter = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            _otpRepositoryMock.Setup(x => x.FindByUserIdAndOtpCode(user.Id, userEnteredCode)).Returns((Otp)null);

            var result = _otpService.VerifyOtp(user, userEnteredCode);

            result.Should().BeFalse();
            _otpRepositoryMock.Verify(x => x.Add(It.IsAny<Otp>()), Times.Never());
        }

        [Fact]
        public void VerifyOtp_ShouldReturnFalse_WhenOtpAlreadyUsedWithinOneTimeWindow()
        {
            var user = new User { Id = "userId", SecretKey = "secretKey" };
            var userEnteredCode = _otpService.GenerateOtpForUser(user);
            var currentCounter = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var usedOtp = new Otp { UserId = user.Id, OtpCode = userEnteredCode, Counter = currentCounter - 1, UsedAt = DateTime.UtcNow };
            _otpRepositoryMock.Setup(x => x.FindByUserIdAndOtpCode(user.Id, userEnteredCode)).Returns(usedOtp);

            var result = _otpService.VerifyOtp(user, userEnteredCode);

            result.Should().BeFalse();
            _otpRepositoryMock.Verify(x => x.Add(It.IsAny<Otp>()), Times.Never());
        }
    }
}

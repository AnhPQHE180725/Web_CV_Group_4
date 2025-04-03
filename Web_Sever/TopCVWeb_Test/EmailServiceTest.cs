using Microsoft.Extensions.Configuration;
using Moq;
using Web_Server.Services;

namespace TopCVWeb_Test
{
    public class EmailServiceTest
    {
        private EmailService _emailService;
        [SetUp]
        public void Setup()
        {
            var mockConfiguration = new Mock<IConfiguration>();

            // Set up mock values for EmailSettings section
            mockConfiguration.Setup(c => c["EmailSettings:SmtpServer"]).Returns("smtp.gmail.com");
            mockConfiguration.Setup(c => c["EmailSettings:Port"]).Returns("587");
            mockConfiguration.Setup(c => c["EmailSettings:SenderEmail"]).Returns("keeplearnedunow@gmail.com");
            mockConfiguration.Setup(c => c["EmailSettings:SenderPassword"]).Returns("tdgi bjdx glap epvs");

            // Create the EmailService with mocked IConfiguration
            _emailService = new EmailService(mockConfiguration.Object);

        }

        //SendPasswordResetEmailAsync_Testing
        [Test]
        public async Task SendPasswordResetEmailAsync_SendMail_ReturnTrue()
        {
            string realEmail = "thisisyouryour@mail.com";
            string resetLink = "http://localhost/reset-password/token";
            var result = await _emailService.SendPasswordResetEmailAsync(realEmail, resetLink);
            Assert.IsTrue(result);
        }

        //SendOtpEmailAsync_Testing
        [Test]
        public async Task SendOtpEmailAsync_SendMail_ReturnTrue()
        {
            string realEmail = "thisisyour@mail.com";
            string otp = "this is otp";
            var result = await _emailService.SendOtpEmailAsync(realEmail, otp);
            Assert.IsTrue(result);
        }


        [TearDown]
        public void TearDown()
        {
        }
    }
}

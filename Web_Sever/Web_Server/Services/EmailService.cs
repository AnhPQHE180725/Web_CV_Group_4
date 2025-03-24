using MailKit.Net.Smtp;
using MimeKit;
using Web_Server.Interfaces;

namespace Web_Server.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        // Inject IConfiguration để lấy thông tin cấu hình từ appsettings.json
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // Phương thức tạo MimeMessage dùng chung
        private MimeMessage CreateEmail(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("CV_Web_Email_Sender",
                _configuration["EmailSettings:SenderEmail"]));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };
            return email;
        }

        // Phương thức gửi email dùng chung
        private async Task SendEmailAsync(MimeMessage email)
        {
            using var smtp = new SmtpClient();

            try
            {
                await smtp.ConnectAsync(
                    _configuration["EmailSettings:SmtpServer"],
                    int.Parse(_configuration["EmailSettings:Port"]),
                    MailKit.Security.SecureSocketOptions.StartTls
                );

                await smtp.AuthenticateAsync(
                    _configuration["EmailSettings:SenderEmail"],
                    _configuration["EmailSettings:SenderPassword"]
                );

                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
            }
            finally
            {
                if (smtp.IsConnected)
                {
                    await smtp.DisconnectAsync(true);
                }
            }
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            var email = CreateEmail(toEmail
                , "Reset Your Password"
                , $@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #000000; margin: 0; padding: 20px;'>
                <div style='max-width: 600px; margin: 0 auto; border: 1px solid #000000; padding: 20px;'>
                    <h2 style='color: #000000; margin-top: 0;'>Password Reset Request</h2>
                    <p style='line-height: 1.6;'>
                        You recently requested to reset your password. Click the button below to proceed:
                    </p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' 
                           style='background-color: #000000; 
                                  color: #ffffff; 
                                  padding: 12px 24px; 
                                  text-decoration: none; 
                                  display: inline-block;
                                  font-weight: bold;'>
                            Reset Password
                        </a>
                    </div>
                </div>
            </body>
            </html>");

            await SendEmailAsync(email);
        }

    }
}

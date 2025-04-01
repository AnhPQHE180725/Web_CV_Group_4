using MailKit.Net.Smtp; // Thư viện hỗ trợ gửi email
using MimeKit;          // Thư viện hỗ trợ tạo email
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
                _configuration["EmailSettings:SenderEmail"]));  // Tên người gửi
            email.To.Add(new MailboxAddress("", toEmail));      // Tên người nhận
            email.Subject = subject;                            // Tiêu đề email
            email.Body = new TextPart("html") { Text = body };  // Nội dung email
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
                // Gửi email
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
            }
            finally
            {
                // Đóng kết nối
                if (smtp.IsConnected)
                {
                    await smtp.DisconnectAsync(true);
                }
            }
        }
        // Phương thức gửi email reset password
        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            // Nội dung email
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

            await SendEmailAsync(email);                // Gửi email

        }


        //  Phương thức gửi mã OTP
        public async Task SendOtpEmailAsync(string toEmail, string otpCode)
        {
            var email = CreateEmail(toEmail,
                "Xác nhận đăng nhập",
                $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>Mã xác minh đăng nhập</h2>
                    <p>
                        Mã xác minh của bạn là: <strong>{otpCode}</strong>
                    </p>
                    <p>
                        Vui lòng nhập mã này vào trang web để hoàn tất quá trình đăng nhập. 
                        Mã sẽ hết hạn sau 5 phút.
                    </p>
                </body>
                </html>");

            await SendEmailAsync(email);
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = CreateEmail(toEmail, subject, body);
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

                // Send email
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
            }
            finally
            {
                // Disconnect SMTP client
                if (smtp.IsConnected)
                {
                    await smtp.DisconnectAsync(true);
                }
            }
        }
    }
}

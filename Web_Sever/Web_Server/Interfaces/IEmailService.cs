namespace Web_Server.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetLink);
        Task<bool> SendOtpEmailAsync(string toEmail, string otpCode);
        Task SendEmailAsync(string to, string subject, string body);
    }
}

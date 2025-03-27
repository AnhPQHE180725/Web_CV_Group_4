namespace Web_Server.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
        Task SendOtpEmailAsync(string toEmail, string otpCode);

    }
}

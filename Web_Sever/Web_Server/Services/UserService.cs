using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.Repositories;
using Web_Server.ViewModels;

namespace Web_Server.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;


        public UserService(IUserRepository repository, IMemoryCache cache, IEmailService emailService)
        {
            _repository = repository;
            _cache = cache;
            _emailService = emailService;
        }

        public async Task<User> CheckLoginAsync(LoginVm loginVm)
        {
            var user = await FindEmailExists(loginVm.Email);
            if (user == null)
            {
                return null;
            }
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, loginVm.Password);

            if (result == PasswordVerificationResult.Success)
            {
                return user; // Đăng nhập thành công
            }

            return null;
        }

        public async Task<User> FindEmailExists(string email)
        {
            return await _repository.FindEmailExists(email);
        }

        public async Task<List<CandidateVm>> GetCandidateByPostId(int id)
        {
            if(id <=0)
            {
                throw new ArgumentOutOfRangeException("id");
            }
            else
            return await _repository.GetCandidateByPostId(id);
            
        }

        public Task<CV> GetCVByUserId(int id)
        {   
            if(id <=0)
            {
                throw new ArgumentOutOfRangeException("id");
            }
            else
            return _repository.GetCVByUserId(id);
        }

        public async Task<bool> RegisterAysnc(RegisterVm registerVm)
        {
            // Kiểm tra định dạng email hợp lệ
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(registerVm.Email))
            {
                return false;
            }
            var User = await FindEmailExists(registerVm.Email);
            if (User != null)
            {
                return false;
            }
            // Mã hóa mật khẩu trước khi lưu
            var passwordHasher = new PasswordHasher<RegisterVm>();
            registerVm.Password = passwordHasher.HashPassword(registerVm, registerVm.Password);

            var result = await _repository.RegisterAysnc(registerVm);
            return result;

        }

        public async Task<User> TakeRoleAsync(User user)
        {
            return await _repository.TakeRoleAsync(user);
        }

        public async Task<bool> ForgotPasswordAsync(string email) // Gửi email reset password
        {
            var user = await _repository.FindEmailExists(email);
            if (user == null) return false;

            // Tạo GUID Token và lưu vào cache với thời gian hết hạn là 1 giờ
            var token = Guid.NewGuid().ToString();
            _cache.Set(token, user.Id, TimeSpan.FromHours(1));

            // Remind: Đường dẫn reset password sẽ được gửi qua email 
            //var resetLink = $"https://localhost:7247/api/Authentication/reset-password?token={token}"; // Đường dẫn reset password (Swagger)
            var resetLink = $"http://localhost:4200/reset-password/{token}";

            await _emailService.SendPasswordResetEmailAsync(email, resetLink);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword) // Đặt lại mật khẩu
        {
            if (!_cache.TryGetValue(token, out int userId)) return false;

            await _repository.UpdatePasswordAsync(userId, newPassword);             // Cập nhật mật khẩu mới

            _cache.Remove(token);    // Xóa token khỏi cache sau khi đặt lại mật khẩu thành công


            return true;
        }
        public async Task<ApplyPost> ApplyCV(int id)
        {
            var applyPost = await _repository.ApplyCV(id);

            if (applyPost.User != null && !string.IsNullOrEmpty(applyPost.User.Email))
            {
                string companyName = applyPost.Recruitment?.Company?.Name ?? "Our Company";

                string subject = "Cập nhật trạng thái ứng tuyển";
                string message = $"Chào {applyPost.User.FullName},<br>" +
                                 $"Hồ sơ của bạn đã được doanh nghiệp <strong>{companyName}</strong> chấp thuận.<br>" +
                                 $"Chúng tôi sẽ liên hệ với bạn thời gian và địa điểm phỏng vấn sớm nhất có thể.<br><br>" +
                                 $"Trân trọng,<br>" +
                                 $"<strong>{companyName}</strong>.";

                await _emailService.SendEmailAsync(applyPost.User.Email, subject, message);
            }

            return applyPost;
        }
        public async Task<ApplyPost> RejectCV(int id)
        {
            var applyPost = await _repository.RejectCV(id);

            if (applyPost.User != null && !string.IsNullOrEmpty(applyPost.User.Email))
            {
                string companyName = applyPost.Recruitment?.Company?.Name ?? "Our Company";

                string subject = "Cập nhật trạng thái ứng tuyển";
                string message = $@"
                                Chào {applyPost.User.FullName},<br><br>
                                Hồ sơ của bạn đã bị doanh nghiệp <strong>{companyName}</strong> từ chối.<br>
                                Chúng tôi rất tiếc và hẹn gặp lại bạn ở những cơ hội việc làm khác.<br><br>
                                Trân trọng,<br>
                                <strong>{companyName}</strong>.
                            ";

                await _emailService.SendEmailAsync(applyPost.User.Email, subject, message);
            }

            return applyPost;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _repository.GetUserByIdAsync(userId);
        }
        public async Task<bool> UpdateProfileAsync(int userId, UserVm model)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null) return false;

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.Description = model.Description;
            user.Image = model.Image;
            user.Email = model.Email;

            return await _repository.UpdateProfileAsync(user);
        }

        public async Task<bool> UpdateEmailAsync(int userId, string newEmail)
        {
            if (await _repository.IsTakenEmailAsync(newEmail))
            {
                return false;
            }

            return await _repository.UpdateEmailAsync(userId, newEmail);
        }

        public async Task<bool> IsTakenEmailAsync(string email)
        {
            return await _repository.IsTakenEmailAsync(email);
        }
    }

}


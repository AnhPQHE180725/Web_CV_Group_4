using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
            var User = await FindEmailExists(loginVm.Email);
            if (User == null)
            {
                return null;
            }
            return await _repository.CheckLoginAsync(loginVm);
        }

        public async Task<User> FindEmailExists(string email)
        {
            return await _repository.FindEmailExists(email);
        }

        public async Task<List<CandidateVm>> GetCandidateByPostId(int id)
        {
            return await _repository.GetCandidateByPostId(id);
        }

        public Task<CV> GetCVByUserId(int id)
        {
            return _repository.GetCVByUserId(id);
        }

        public async Task<bool> RegisterAysnc(RegisterVm registerVm)
        {
            var User = await FindEmailExists(registerVm.Email);
            if(User != null)
            {
                return false;
            }
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
            var resetLink = $"https://localhost:7247/api/Authentication/reset-password?token={token}"; // Đường dẫn reset password (Swagger)

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
            return await _repository.ApplyCV(id);
        }
        public async Task<ApplyPost> RejectCV(int id)
        {
            return await _repository.RejectCV(id);
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


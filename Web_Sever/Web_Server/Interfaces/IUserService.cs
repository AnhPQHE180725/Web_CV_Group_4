using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Interfaces
{
    public interface IUserService
    {
        Task<User?> FindEmailExists(string email);
        Task<User> CheckLoginAsync(LoginVm loginVm);
        Task<bool> RegisterAysnc(RegisterVm registerVm);
        Task<User> TakeRoleAsync(User user);

        Task<List<CandidateVm>> GetCandidateByPostId(int id);
        Task<CV> GetCVByUserId(int id);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);

        Task<ApplyPost> ApplyCV(int id);
        Task<ApplyPost> RejectCV(int id);
        
    }
}

using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> FindEmailExists(string email);
        Task<User> CheckLoginAsync(LoginVm loginVm);
        Task<bool> RegisterAysnc(RegisterVm registerVm);
        Task<User> TakeRoleAsync(User user);

        Task<List<CandidateVm>> GetCandidateByPostId(int id);

        Task<CV> GetCVByUserId(int id);

    }
}

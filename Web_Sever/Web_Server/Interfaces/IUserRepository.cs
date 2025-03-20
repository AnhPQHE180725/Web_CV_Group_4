using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> FindEmailExists(string email);
        Task<User> CheckLoginAsync(LoginVm loginVm);
    }
}

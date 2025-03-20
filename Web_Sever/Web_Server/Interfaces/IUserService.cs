using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Interfaces
{
    public interface IUserService
    {
        Task<User> CheckLoginAsync(LoginVm loginVm);

    }
}

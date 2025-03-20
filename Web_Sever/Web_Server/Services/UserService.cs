using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Services
{
    public class UserService : IUserService
    {
        public Task<User> CheckLoginAsync(LoginVm loginVm)
        {
            throw new NotImplementedException();
        }
    }
}

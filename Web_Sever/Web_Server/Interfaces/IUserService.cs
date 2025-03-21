﻿using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Interfaces
{
    public interface IUserService
    {
        Task<User> FindEmailExists(string email);
        Task<User> CheckLoginAsync(LoginVm loginVm);
        Task<bool> RegisterAysnc(RegisterVm registerVm);
        Task<User> TakeRoleAsync(User user);

    }
}

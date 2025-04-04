﻿using Web_Server.Models;
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
        Task UpdatePasswordAsync(int userId, string newPassword);

        Task<ApplyPost> ApplyCV(int id);
        Task<ApplyPost> RejectCV(int id);


        Task<User> GetUserByIdAsync(int id);

        Task<bool> UpdateProfileAsync(User user);
        Task<bool> UpdateEmailAsync(int userId, string email);
        Task<bool> IsTakenEmailAsync(string email);
    }
}

using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Interfaces
{
    public interface IApplyPostService
    {
        Task<ApplyPost> ApplyWithNewCVAsync(ApplyWithNewCVVm applyVm);
        Task<ApplyPost> ApplyWithExistingCVAsync(ApplyWithExistingCVVm applyVm);
        Task<List<ApplyPost>> GetApplicationsByUserIdAsync(int userId);
        Task<List<ApplyPost>> GetApplicationsByPostIdAsync(int postId);
        Task<bool> UpdateApplicationStatusAsync(int applicationId, int status);
    }
} 
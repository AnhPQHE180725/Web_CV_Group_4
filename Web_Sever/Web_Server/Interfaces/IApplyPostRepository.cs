using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface IApplyPostRepository
    {
        Task<ApplyPost> GetByIdAsync(int id);
        Task<List<ApplyPost>> GetByRecruitmentIdAsync(int recruitmentId);
        Task<List<ApplyPost>> GetByUserIdAsync(int userId);
        Task<List<ApplyPost>> GetByPostIdAsync(int postId);
        Task<ApplyPost> GetByUserIdAndPostIdAsync(int userId, int postId);
        Task<ApplyPost> CreateAsync(ApplyPost applyPost);
        Task<ApplyPost> UpdateAsync(ApplyPost applyPost);
        Task<bool> DeleteAsync(int id);
    }
} 
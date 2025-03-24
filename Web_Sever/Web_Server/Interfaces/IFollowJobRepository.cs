using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface IFollowJobRepository
    {
        Task<FollowJob> GetByIdAsync(int id);
        Task<List<FollowJob>> GetByUserIdAsync(int userId);
        Task<FollowJob> GetByUserIdAndRecruitmentIdAsync(int userId, int recruitmentId);
        Task<FollowJob> CreateAsync(FollowJob followJob);
        Task<bool> DeleteAsync(int id);
    }
}

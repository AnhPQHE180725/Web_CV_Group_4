using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface IFollowRepository
    {
        Task<Follow> GetByIdAsync(int id);
        Task<List<Follow>> GetByUserIdAsync(int userId);
        Task<Follow> GetByUserIdAndPostIdAsync(int userId, int postId);
        Task<Follow> GetByUserIdAndCompanyIdAsync(int userId, int companyId);
        Task<Follow> CreateAsync(Follow follow);
        Task<bool> DeleteAsync(int id);
    }
}

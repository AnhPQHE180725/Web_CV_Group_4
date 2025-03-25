using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface IFollowCompanyRepository
    {
        Task<FollowCompany> GetByIdAsync(int id);
        Task<List<FollowCompany>> GetByUserIdAsync(int userId);
        Task<FollowCompany> GetByUserIdAndCompanyIdAsync(int userId, int companyId);
        Task<FollowCompany> CreateAsync(FollowCompany followCompany);
        Task<bool> DeleteAsync(int id);
    }
}

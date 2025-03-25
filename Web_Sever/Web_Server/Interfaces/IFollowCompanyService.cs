using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface IFollowCompanyService
    {
        Task<(bool success, string message)> ToggleFollowCompanyAsync(int companyId);
        Task<List<FollowCompany>> GetUserFollowCompaniesAsync();
        Task<bool> IsFollowingCompanyAsync(int companyId);
        Task<List<int>> GetFollowedCompanyIdsAsync();
    }
}

using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface IFollowService
    {
        Task<(bool success, string message)> ToggleFollowPostAsync(int postId);
        Task<(bool success, string message)> ToggleFollowCompanyAsync(int companyId);
        Task<List<Follow>> GetUserFollowsAsync();
        Task<bool> IsFollowingPostAsync(int postId);
        Task<bool> IsFollowingCompanyAsync(int companyId);
    }
}

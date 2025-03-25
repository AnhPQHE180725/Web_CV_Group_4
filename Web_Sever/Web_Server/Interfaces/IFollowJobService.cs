using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface IFollowJobService
    {
        Task<(bool success, string message)> ToggleFollowJobAsync(int recruitmentId);
        Task<List<FollowJob>> GetUserFollowJobsAsync();
        Task<bool> IsFollowingJobAsync(int recruitmentId);
        Task<List<int>> GetFollowedJobIdsAsync();
    }
}

using Web_Server.Interfaces;
using Web_Server.Models;

namespace Web_Server.Services
{
    public class FollowJobService : IFollowJobService
    {
        private readonly IFollowJobRepository _followJobRepository;
        private readonly IRecruitmentRepository _recruitmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FollowJobService(
            IFollowJobRepository followJobRepository,
            IRecruitmentRepository recruitmentRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _followJobRepository = followJobRepository;
            _recruitmentRepository = recruitmentRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<int> GetCurrentUserIdAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                throw new UnauthorizedAccessException("Invalid user ID in token");

            return userId;
        }

        public async Task<(bool success, string message)> ToggleFollowJobAsync(int recruitmentId)
        {
            var userId = await GetCurrentUserIdAsync();
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found");

            var recruitment = await _recruitmentRepository.GetByIdAsync(recruitmentId);
            if (recruitment == null)
                return (false, "Recruitment not found");

            var existingFollow = await _followJobRepository.GetByUserIdAndRecruitmentIdAsync(userId, recruitmentId);
            if (existingFollow != null)
            {
                // Unfollow
                var result = await _followJobRepository.DeleteAsync(existingFollow.Id);
                return (result, "Bỏ lưu theo dõi thành công");
            }
            else
            {
                // Follow
                var newFollow = new FollowJob
                {
                    UserId = userId,
                    RecruitmentId = recruitmentId,
                };
                await _followJobRepository.CreateAsync(newFollow);
                return (true, "Lưu theo dõi thành công");
            }
        }

        public async Task<List<FollowJob>> GetUserFollowJobsAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            return await _followJobRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> IsFollowingJobAsync(int recruitmentId)
        {
            var userId = await GetCurrentUserIdAsync();
            var follow = await _followJobRepository.GetByUserIdAndRecruitmentIdAsync(userId, recruitmentId);
            return follow != null;
        }

        public async Task<List<int>> GetFollowedJobIdsAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            var follows = await _followJobRepository.GetByUserIdAsync(userId);
            return follows.Select(f => f.RecruitmentId).ToList();
        }
    }
}
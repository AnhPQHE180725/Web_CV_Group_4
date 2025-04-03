using Web_Server.Interfaces;
using Web_Server.Models;

namespace Web_Server.Services
{
    public class FollowCompanyService : IFollowCompanyService
    {
        private readonly IFollowCompanyRepository _followCompanyRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FollowCompanyService(
            IFollowCompanyRepository followCompanyRepository,
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _followCompanyRepository = followCompanyRepository;
            _companyRepository = companyRepository;
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

        public async Task<(bool success, string message)> ToggleFollowCompanyAsync(int companyId)
        {
            var userId = await GetCurrentUserIdAsync();
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found");

            var company = await _companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return (false, "Company not found");

            var existingFollow = await _followCompanyRepository.GetByUserIdAndCompanyIdAsync(userId, companyId);
            if (existingFollow != null)
            {
                // Unfollow
                var result = await _followCompanyRepository.DeleteAsync(existingFollow.Id);
                return (result, "Bỏ lưu theo dõi thành công");
            }
            else
            {
                // Follow
                var newFollow = new FollowCompany
                {
                    UserId = userId,
                    CompanyId = companyId,
                };
                await _followCompanyRepository.CreateAsync(newFollow);
                return (true, "Lưu theo dõi thành công");
            }
        }

        public async Task<List<FollowCompany>> GetUserFollowCompaniesAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            return await _followCompanyRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> IsFollowingCompanyAsync(int companyId)
        {
            var userId = await GetCurrentUserIdAsync();
            var follow = await _followCompanyRepository.GetByUserIdAndCompanyIdAsync(userId, companyId);
            return follow != null;
        }

        public async Task<List<int>> GetFollowedCompanyIdsAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            var follows = await _followCompanyRepository.GetByUserIdAsync(userId);
            return follows.Select(f => f.CompanyId).ToList();
        }
    }
}
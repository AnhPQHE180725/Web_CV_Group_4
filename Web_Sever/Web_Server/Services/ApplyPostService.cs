using Microsoft.AspNetCore.Http;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Services
{
    public class ApplyPostService : IApplyPostService
    {
        private readonly IApplyPostRepository _applyPostRepository;
        private readonly IRecruitmentRepository _recruitmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICVService _cvService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplyPostService(
            IApplyPostRepository applyPostRepository,
            IRecruitmentRepository recruitmentRepository,
            IUserRepository userRepository,
            ICVService cvService,
            IHttpContextAccessor httpContextAccessor)
        {
            _applyPostRepository = applyPostRepository;
            _recruitmentRepository = recruitmentRepository;
            _userRepository = userRepository;
            _cvService = cvService;
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

        private async Task ValidatePostAsync(int postId)
        {
            var post = await _recruitmentRepository.GetByIdAsync(postId);
            if (post == null)
                throw new ArgumentException($"Post with ID {postId} not found");

            if (post.Status == 0)
                throw new InvalidOperationException("This post is no longer active");
        }

        private async Task ValidateUserHasNotAppliedAsync(int userId, int postId)
        {
            var existingApplication = await _applyPostRepository.GetByUserIdAndPostIdAsync(userId, postId);
            if (existingApplication != null)
                throw new InvalidOperationException("You have already applied for this post");
        }

        public async Task<ApplyPost> ApplyWithNewCVAsync(ApplyWithNewCVVm applyVm)
        {
            if (applyVm == null)
                throw new ArgumentNullException(nameof(applyVm), "Apply data cannot be null");

            if (applyVm.PostId <= 0)
                throw new ArgumentException("Invalid post ID", nameof(applyVm.PostId));

            if (applyVm.CVFile == null)
                throw new ArgumentException("CV file is required", nameof(applyVm.CVFile));

            if (applyVm.CVFile.Length == 0)
                throw new ArgumentException("CV file cannot be empty", nameof(applyVm.CVFile));

            if (!applyVm.CVFile.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Only PDF files are allowed", nameof(applyVm.CVFile));

            if (applyVm.CVFile.Length > 5 * 1024 * 1024) // 5MB limit
                throw new ArgumentException("CV file size cannot exceed 5MB", nameof(applyVm.CVFile));

            var userId = await GetCurrentUserIdAsync();
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");

            await ValidatePostAsync(applyVm.PostId);
            await ValidateUserHasNotAppliedAsync(userId, applyVm.PostId);

            try
            {
                // Upload new CV
                var cv = await _cvService.UploadCVAsync(applyVm.CVFile);
                
                // Create new application with new CV
                var newApplication = new ApplyPost
                {
                    UserId = userId,
                    RecruitmentId = applyVm.PostId,
                    CVName = cv.Name,
                    Status = 0,
                    CreatedAt = DateTime.UtcNow,
                    Text = applyVm.Text
                };

                return await _applyPostRepository.CreateAsync(newApplication);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to process new CV application: {ex.InnerException}");
            }
        }

        public async Task<ApplyPost> ApplyWithExistingCVAsync(ApplyWithExistingCVVm applyVm)
        {
            if (applyVm == null)
                throw new ArgumentNullException(nameof(applyVm), "Apply data cannot be null");

            if (applyVm.PostId <= 0)
                throw new ArgumentException("Invalid post ID", nameof(applyVm.PostId));

            var userId = await GetCurrentUserIdAsync();
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");

            await ValidatePostAsync(applyVm.PostId);
            await ValidateUserHasNotAppliedAsync(userId, applyVm.PostId);

            try
            {
                // Check if user has an existing CV
                var existingCV = await _cvService.GetCVByUserIdAsync(userId);
                if (existingCV == null)
                    throw new InvalidOperationException("You need to upload a CV first before applying with existing CV");

                // Create new application with existing CV
                var newApplication = new ApplyPost
                {
                    UserId = userId,
                    RecruitmentId = applyVm.PostId,
                    CVName = existingCV.Name,
                    Status = 0,
                    CreatedAt = DateTime.UtcNow,
                    Text = applyVm.Text
                };

                return await _applyPostRepository.CreateAsync(newApplication);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to process existing CV application: {ex.Message}");
            }
        }

        public async Task<List<ApplyPost>> GetApplicationsByUserIdAsync(int userId)
        {
            return await _applyPostRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<ApplyPost>> GetApplicationsByPostIdAsync(int postId)
        {
            return await _applyPostRepository.GetByPostIdAsync(postId);
        }

        public async Task<bool> UpdateApplicationStatusAsync(int applicationId, int status)
        {
            var application = await _applyPostRepository.GetByIdAsync(applicationId);
            if (application == null)
                return false;

            application.Status = status;
            return await _applyPostRepository.UpdateAsync(application) != null;
        }
    }
} 
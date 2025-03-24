using Microsoft.AspNetCore.Http;
using Web_Server.Models;
using Web_Server.Interfaces;

namespace Web_Server.Services
{
    public class CVService : ICVService
    {
        private readonly ICVRepository _cvRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CV_FOLDER = "CVs";

        public CVService(
            ICVRepository cvRepository, 
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor)
        {
            _cvRepository = cvRepository;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CV> GetCVByIdAsync(int id)
        {
            return await _cvRepository.GetByIdAsync(id);
        }

        public async Task<CV> GetCVByUserIdAsync(int userId)
        {
            return await _cvRepository.GetByUserIdAsync(userId);
        }

        public async Task<CV> UploadCVAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded");

            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Only PDF files are allowed");

            if (string.IsNullOrEmpty(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            // Create wwwroot folder if it doesn't exist
            if (!Directory.Exists(_environment.WebRootPath))
            {
                Directory.CreateDirectory(_environment.WebRootPath);
            }

            // Create CVs folder if it doesn't exist
            var cvFolderPath = Path.Combine(_environment.WebRootPath, CV_FOLDER);
            if (!Directory.Exists(cvFolderPath))
                Directory.CreateDirectory(cvFolderPath);

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(cvFolderPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Get userId from current user's token
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("id");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                throw new UnauthorizedAccessException("Invalid user ID in token");

            // Check if user already has a CV
            var existingCV = await _cvRepository.GetByUserIdAsync(userId);
            if (existingCV != null)
            {
                // Delete old file
                var oldFilePath = Path.Combine(cvFolderPath, existingCV.Name);
                if (File.Exists(oldFilePath))
                    File.Delete(oldFilePath);

                // Update existing CV
                existingCV.Name = fileName;
                return await _cvRepository.UpdateAsync(existingCV);
            }

            // Create new CV
            var newCV = new CV
            {
                Name = fileName,
                UserId = userId
            };

            return await _cvRepository.CreateAsync(newCV);
        }

        public async Task<bool> DeleteCVAsync(int id)
        {
            var cv = await _cvRepository.GetByIdAsync(id);
            if (cv == null)
                return false;

            if (string.IsNullOrEmpty(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            // Delete file
            var filePath = Path.Combine(_environment.WebRootPath, CV_FOLDER, cv.Name);
            if (File.Exists(filePath))
                File.Delete(filePath);

            return await _cvRepository.DeleteAsync(id);
        }
    }
} 
using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface ICVService
    {
        Task<CV> GetCVByIdAsync(int id);
        Task<CV> GetCVByUserIdAsync(int userId);
        Task<CV> UploadCVAsync(IFormFile file);
        Task<bool> DeleteCVAsync(int id);
    }
} 
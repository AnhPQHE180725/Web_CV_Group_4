using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface ICVRepository
    {
        Task<CV> GetByIdAsync(int id);
        Task<CV> GetByUserIdAsync(int userId);
        Task<CV> CreateAsync(CV cv);
        Task<CV> UpdateAsync(CV cv);
        Task<bool> DeleteAsync(int id);
    }
} 
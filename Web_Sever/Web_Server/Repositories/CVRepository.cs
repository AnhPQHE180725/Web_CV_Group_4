using Microsoft.EntityFrameworkCore;
using Web_Server.Data;
using Web_Server.Interfaces;
using Web_Server.Models;

namespace Web_Server.Repositories
{
    public class CVRepository : ICVRepository
    {
        private readonly AppDbContext _context;

        public CVRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CV> GetByIdAsync(int id)
        {
            return await _context.CVs.FindAsync(id);
        }

        public async Task<CV> GetByUserIdAsync(int userId)
        {
            return await _context.CVs.FirstOrDefaultAsync(cv => cv.UserId == userId);
        }

        public async Task<CV> CreateAsync(CV cv)
        {
            await _context.CVs.AddAsync(cv);
            await _context.SaveChangesAsync();
            return cv;
        }

        public async Task<CV> UpdateAsync(CV cv)
        {
            _context.CVs.Update(cv);
            await _context.SaveChangesAsync();
            return cv;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cv = await _context.CVs.FindAsync(id);
            if (cv == null)
                return false;

            _context.CVs.Remove(cv);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 
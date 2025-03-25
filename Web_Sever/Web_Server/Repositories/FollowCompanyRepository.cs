using Microsoft.EntityFrameworkCore;
using Web_Server.Data;
using Web_Server.Interfaces;
using Web_Server.Models;

namespace Web_Server.Repositories
{
    public class FollowCompanyRepository : IFollowCompanyRepository
    {
        private readonly AppDbContext _context;

        public FollowCompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FollowCompany> GetByIdAsync(int id)
        {
            return await _context.FollowCompanies
                .Include(f => f.User)
                .Include(f => f.Company)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<FollowCompany>> GetByUserIdAsync(int userId)
        {
            return await _context.FollowCompanies
                .Include(f => f.User)
                .Include(f => f.Company)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<FollowCompany> GetByUserIdAndCompanyIdAsync(int userId, int companyId)
        {
            return await _context.FollowCompanies
                .Include(f => f.User)
                .Include(f => f.Company)
                .FirstOrDefaultAsync(f => f.UserId == userId && f.CompanyId == companyId);
        }

        public async Task<FollowCompany> CreateAsync(FollowCompany followCompany)
        {
            _context.FollowCompanies.Add(followCompany);
            await _context.SaveChangesAsync();
            return followCompany;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var followCompany = await _context.FollowCompanies.FindAsync(id);
            if (followCompany == null)
                return false;

            _context.FollowCompanies.Remove(followCompany);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

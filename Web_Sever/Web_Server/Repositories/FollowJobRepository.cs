using Microsoft.EntityFrameworkCore;
using Web_Server.Data;
using Web_Server.Interfaces;
using Web_Server.Models;

namespace Web_Server.Repositories
{
    public class FollowJobRepository : IFollowJobRepository
    {
        private readonly AppDbContext _context;

        public FollowJobRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FollowJob> GetByIdAsync(int id)
        {
            return await _context.FollowJobs
                .Include(f => f.User)
                .Include(f => f.Recruitment)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<FollowJob>> GetByUserIdAsync(int userId)
        {
            return await _context.FollowJobs
                .Include(f => f.User)
                .Include(f => f.Recruitment)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<FollowJob> GetByUserIdAndRecruitmentIdAsync(int userId, int recruitmentId)
        {
            return await _context.FollowJobs
                .Include(f => f.User)
                .Include(f => f.Recruitment)
                .FirstOrDefaultAsync(f => f.UserId == userId && f.RecruitmentId == recruitmentId);
        }

        public async Task<FollowJob> CreateAsync(FollowJob followJob)
        {
            _context.FollowJobs.Add(followJob);
            await _context.SaveChangesAsync();
            return followJob;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var followJob = await _context.FollowJobs.FindAsync(id);
            if (followJob == null)
                return false;

            _context.FollowJobs.Remove(followJob);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Web_Server.Data;
using Web_Server.Models;
using Web_Server.Interfaces;

namespace Web_Server.Repositories
{
    public class ApplyPostRepository : IApplyPostRepository
    {
        private readonly AppDbContext _context;

        public ApplyPostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApplyPost> GetByIdAsync(int id)
        {
            return await _context.ApplyPosts
                .Include(a => a.User)
                .Include(a => a.Recruitment)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<ApplyPost>> GetByRecruitmentIdAsync(int recruitmentId)
        {
            return await _context.ApplyPosts
                .Include(a => a.User)
                .Where(a => a.RecruitmentId == recruitmentId)
                .ToListAsync();
        }

        public async Task<List<ApplyPost>> GetByUserIdAsync(int userId)
        {
            return await _context.ApplyPosts
                .Include(a => a.Recruitment)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<ApplyPost>> GetByPostIdAsync(int postId)
        {
            return await _context.ApplyPosts
                .Include(a => a.User)
                .Where(a => a.RecruitmentId == postId)
                .ToListAsync();
        }

        public async Task<ApplyPost> GetByUserIdAndPostIdAsync(int userId, int postId)
        {
            return await _context.ApplyPosts
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.UserId == userId && a.RecruitmentId == postId);
        }

        public async Task<ApplyPost> CreateAsync(ApplyPost applyPost)
        {
            await _context.ApplyPosts.AddAsync(applyPost);
            await _context.SaveChangesAsync();
            return applyPost;
        }

        public async Task<ApplyPost> UpdateAsync(ApplyPost applyPost)
        {
            _context.ApplyPosts.Update(applyPost);
            await _context.SaveChangesAsync();
            return applyPost;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var applyPost = await _context.ApplyPosts.FindAsync(id);
            if (applyPost == null)
                return false;

            _context.ApplyPosts.Remove(applyPost);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 
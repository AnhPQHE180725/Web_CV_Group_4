using Microsoft.EntityFrameworkCore;
using Web_Server.Data;
using Web_Server.Interfaces;
using Web_Server.Models;

namespace Web_Server.Repositories
{
    public class RecruitmentRepository : IRecruitmentRepository
    {
        private readonly AppDbContext _context;

        public RecruitmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Recruitment>> GetAllRecruitments()
        {
            return await _context.Recruitments.ToListAsync();   
        }

        public async Task<List<Recruitment>> GetRecruitmentsByCategory(int id)
        {
            return await _context.Recruitments.Where(c=>c.CategoryId == id).ToListAsync();
        }

        public async Task<List<Recruitment>> GetRecruitmentsByCompany(int id)
        {
            return await _context.Recruitments.Where(c=>c.CompanyId ==id).ToListAsync();
        }

        public async Task<List<Recruitment>> GetTop2Recruitments()
        {
            return await _context.Recruitments.OrderByDescending(c=>c.View).Take(2).ToListAsync();
        }
    }
}

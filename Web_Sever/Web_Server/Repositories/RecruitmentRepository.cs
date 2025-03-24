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
            return await _context.Recruitments
          .Where(r => r.CategoryId == id)
          .Include(r => r.Company) 
          .Include(r => r.Category) 
          .ToListAsync();
        }

        public async Task<List<Recruitment>> GetRecruitmentsByCompany(int id)
        {
            return await _context.Recruitments
          .Where(r => r.CompanyId == id)
          .Include(r => r.Company)
          .Include(r => r.Category)
          .ToListAsync();
        }

        public async Task<List<Recruitment>> GetTop2Recruitments()
        {
            return await _context.Recruitments.OrderByDescending(c=>c.View).Take(2).ToListAsync();
        }
        public async Task<List<Recruitment>> GetRecruitmentsByCompanyName(string company)
        {
            return await _context.Recruitments.Where(c => c.Company.Name.Contains(company)).ToListAsync();
        }
        public async Task<List<Recruitment>> GetRecruitmentsByTitle(string title)
        {
            return await _context.Recruitments.Where(t => t.Title.Contains(title)).ToListAsync();
        }
        public async Task<List<Recruitment>> GetRecruitmentsByLocation(string location)
        {
            return await _context.Recruitments.Where(t => t.Address.Contains(location)).ToListAsync();
        }

        public async Task<Recruitment> GetByIdAsync(int id)
        {
            return await _context.Recruitments
                .Include(r => r.Company)
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}

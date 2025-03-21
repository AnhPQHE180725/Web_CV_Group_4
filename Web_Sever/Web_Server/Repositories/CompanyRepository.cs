using Microsoft.EntityFrameworkCore;
using Web_Server.Data;
using Web_Server.Interfaces;
using Web_Server.Models;

namespace Web_Server.Repositories
{
    public class CompanyRepository : ICompanyRepository

    {
        private readonly AppDbContext _context;
        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Company>> GetAllCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<List<Company>> GetTop4Companies()
        {
            return await _context.Companies.OrderByDescending(c=>c.Recruitments.Count).Take(4).ToListAsync();
        }
        public async Task<List<Company>> GetCompaniesByName(string name)
        {
            return await _context.Companies.Where(c => c.Name.Contains(name)).ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Web_Server.Data;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Repositories
{
    public class CompanyRepository : ICompanyRepository

    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            return await _context.Companies
                .Include(c => c.Recruitments)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Company>> GetAllAsync()
        {
            return await _context.Companies
                .Include(c => c.Recruitments)
                .ToListAsync();
        }

        public async Task<Company> CreateAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company> UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return false;

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Company>> GetAllCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<List<Company>> GetTop4Companies()
        {
            return await _context.Companies.OrderByDescending(c => c.Recruitments.Count).Take(4).ToListAsync();
        }
        public async Task<List<Company>> GetCompaniesByName(string name)
        {
            return await _context.Companies.Where(c => c.Name.Contains(name)).ToListAsync();
        }

        public async Task<List<Company>> GetCompaniesByUserIdAsync(int userId)
        {
            return await _context.Companies
                .Where(c => c.UserId == userId)
                .Include(c => c.Recruitments)
                .ToListAsync();
        }
        public async Task<Company> GetCompanyProfileAsync()
        {
            return await _context.Companies.FirstOrDefaultAsync();
        }
    }
}

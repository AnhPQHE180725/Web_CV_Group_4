using Microsoft.EntityFrameworkCore;
using Web_Server.Data;
using Web_Server.Interfaces;
using Web_Server.Models;

namespace Web_Server.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> Get5TopCategories()
        {
            return await _context.Categories.OrderByDescending(c => c.Recruitments.Count()).Take(5).ToListAsync();
        }

        public async Task<List<Category>> GetAllCategories()
        {
           return await _context.Categories.ToListAsync();
        }


    }
}

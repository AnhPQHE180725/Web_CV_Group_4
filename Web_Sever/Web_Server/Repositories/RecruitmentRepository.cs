﻿using Microsoft.EntityFrameworkCore;
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
        public async Task<bool> AddRecruitmentAsync(Recruitment recruitment)
        {
            await _context.Recruitments.AddAsync(recruitment);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Recruitment> GetRecruitmentByIdAsync(int id)
        {
            return await _context.Recruitments.FindAsync(id);
        }
        public async Task<bool> EditRecruitmentAsync(Recruitment recruitment)
        {
            _context.Recruitments.Update(recruitment);  // Cập nhật đối tượng
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteRecruitmentAsync(int id)
        {
            var recruitment = await _context.Recruitments.FindAsync(id);
            if (recruitment == null) return false;

            _context.Recruitments.Remove(recruitment);  // Xóa đối tượng
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

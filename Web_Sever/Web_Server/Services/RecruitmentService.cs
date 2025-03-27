using Microsoft.EntityFrameworkCore;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Services
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly IRecruitmentRepository _repository;

        public RecruitmentService(IRecruitmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Recruitment>> GetAllRecruitments()
        {
            return await _repository.GetAllRecruitments();
        }

        public async Task<List<RecruitmentVm>> GetRecruitmentsByCategory(int id)
        {
            var recruitments = await _repository.GetRecruitmentsByCategory(id);

            return recruitments.Select(r => new RecruitmentVm
            {
                Id = r.Id,
                Address = r.Address,
                CreatedAt = r.CreatedAt,
                Description = r.Description,
                Experience = r.Experience,
                Quantity = r.Quantity,
                Rank = r.Rank,
                Salary = r.Salary,
                Status = r.Status,
                Title = r.Title,
                Type = r.Type,
                View = r.View,
                Deadline = r.Deadline,

                CompanyName = r.Company?.Name ?? "Unknown",
                CategoryName = r.Category?.Name ?? "Unknown"

            }).ToList();
        }

        public async Task<List<RecruitmentVm>> GetRecruitmentsByCompany(int id)
        {
            var recruitments = await _repository.GetRecruitmentsByCompany(id);

            return recruitments.Select(r => new RecruitmentVm
            {
                Id = r.Id,
                Address = r.Address,
                CreatedAt = r.CreatedAt,
                Description = r.Description,
                Experience = r.Experience,
                Quantity = r.Quantity,
                Rank = r.Rank,
                Salary = r.Salary,
                Status = r.Status,
                Title = r.Title,
                Type = r.Type,
                View = r.View,
                Deadline =  r.Deadline,
                CompanyName = r.Company?.Name ?? "Unknown",
                CategoryName = r.Category?.Name ?? "Unknown"
            }).ToList();
        }

        public async Task<List<Recruitment>> GetTop2Recruitments()
        {
            return await _repository.GetTop2Recruitments();
        }

        public async Task<List<Recruitment>> GetRecruitmentsByCompanyName(string company)
        {
            return await _repository.GetRecruitmentsByCompanyName(company);
        }

        public async Task<List<Recruitment>> GetRecruitmentsByTitle(string title)
        {
            return await _repository.GetRecruitmentsByTitle(title);
        }

        public async Task<List<Recruitment>> GetRecruitmentsByLocation(string location)
        {
            return await _repository.GetRecruitmentsByLocation(location);
        }

        public async Task<List<RecruitmentVm>> GetRecruitmentsByid(int id)
        {
            var recruitments = await _repository.GetRecruitmentsByCategory(id);

            return recruitments.Select(r => new RecruitmentVm
            {
                Id = r.Id,
                Address = r.Address,
                CreatedAt = r.CreatedAt,
                Description = r.Description,
                Experience = r.Experience,
                Quantity = r.Quantity,
                Rank = r.Rank,
                Salary = r.Salary,
                Status = r.Status,
                Title = r.Title,
                Type = r.Type,
                View = r.View,
                Deadline = r.Deadline,

                CompanyName = r.Company?.Name ?? "Unknown",
                CategoryName = r.Category?.Name ?? "Unknown"

            }).ToList();
        }

        public async Task<bool> AddRecruitmentAsync(RecruitmentVm recruitmentVm)
        {
            var recruitment = new Recruitment
            {
                Title = recruitmentVm.Title,
                Description = recruitmentVm.Description,
                Salary = recruitmentVm.Salary,
                Status = recruitmentVm.Status,
                Type = recruitmentVm.Type,
                Experience = recruitmentVm.Experience,
                CompanyId = recruitmentVm.CompanyId,
                CategoryId = recruitmentVm.CategoryId,
                CreatedAt = DateTime.Now,
                Quantity = recruitmentVm.Quantity,
                Deadline = recruitmentVm.Deadline,
                Address = recruitmentVm.Address,
                Rank = recruitmentVm.Rank,
                View = 0
            };

            return await _repository.AddRecruitmentAsync(recruitment);
        }

        public async Task<bool> EditRecruitmentAsync(int id, RecruitmentVm recruitmentVm)
        {
            var existingRecruitment = await _repository.GetRecruitmentByIdAsync(id);
            if (existingRecruitment == null) return false;

            existingRecruitment.Title = recruitmentVm.Title;
            existingRecruitment.Description = recruitmentVm.Description;
            existingRecruitment.Salary = recruitmentVm.Salary;
            existingRecruitment.Status = recruitmentVm.Status;
            existingRecruitment.Type = recruitmentVm.Type;
            existingRecruitment.Experience = recruitmentVm.Experience;
            existingRecruitment.CompanyId = recruitmentVm.CompanyId;
            existingRecruitment.CategoryId = recruitmentVm.CategoryId;
            existingRecruitment.Quantity = recruitmentVm.Quantity;
            existingRecruitment.Deadline = recruitmentVm.Deadline;
            existingRecruitment.Address = recruitmentVm.Address;
            existingRecruitment.Rank = recruitmentVm.Rank;

            return await _repository.EditRecruitmentAsync(existingRecruitment);
        }

        public async Task<bool> DeleteRecruitmentAsync(int id)
        {
            return await _repository.DeleteRecruitmentAsync(id);
        }
    }
}
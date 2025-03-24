using Microsoft.EntityFrameworkCore;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Services
{
    public class RecruitmentService:IRecruitmentService
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
                Deadline = r.Deadline,
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
    }
}

    using Microsoft.EntityFrameworkCore;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Services
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly IRecruitmentRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Dictionary để lưu trữ việc xem bài tuyển dụng gần đây (IP + RecruitmentId -> Thời gian xem)
        private static readonly Dictionary<string, DateTime> ViewRecords = new Dictionary<string, DateTime>();
        // Thời gian tối thiểu giữa các lần tăng view (phút)
        private const int ViewCooldownMinutes = 2;


        public RecruitmentService(IRecruitmentRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<Recruitment> GetRecruitmentById(int id)
        {
            return await _repository.GetRecruitmentByIdAsync(id);
        }

        //public async Task<bool> UpdateRecruitmentView(int id)
        //{
        //    var recruitment = await _repository.GetRecruitmentByIdAsync(id);
        //    if (recruitment == null) return false;

        //    recruitment.View += 1;

        //    return await _repository.EditRecruitmentAsync(recruitment);
        //}


        //sd dia chi IP cua ng dung
        public async Task<bool> UpdateRecruitmentView(int id)
        {
            // Lấy địa chỉ IP của người xem
            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // Tạo khóa duy nhất cho IP và bài tuyển dụng
            string viewKey = $"{ipAddress}_{id}";

            // Kiểm tra xem người dùng đã xem gần đây chưa
            if (ViewRecords.TryGetValue(viewKey, out DateTime lastViewed))
            {
                // Nếu chưa đủ thời gian cooldown, không tăng lượt xem
                if ((DateTime.UtcNow - lastViewed).TotalMinutes < ViewCooldownMinutes)
                {
                    // Trả về true nhưng không thực sự cập nhật view
                    return true;
                }
            }

            // Cập nhật thời gian xem gần nhất
            ViewRecords[viewKey] = DateTime.UtcNow;

            // Tăng lượt xem
            var recruitment = await _repository.GetRecruitmentByIdAsync(id);
            if (recruitment == null) return false;

            recruitment.View += 1;

            return await _repository.EditRecruitmentAsync(recruitment);
        }

    }
}
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Interfaces
{
    public interface IRecruitmentService
    {
        Task<List<RecruitmentVm>> GetAllRecruitments();

        Task<List<RecruitmentVm>> GetTop2Recruitments();
        Task<List<RecruitmentVm>> GetRecruitmentsByCompany(int id);
        Task<List<RecruitmentVm>> GetRecruitmentsByCategory(int id);

        Task<List<RecruitmentVm>> GetRecruitmentsByCompanyName(string company);
        Task<List<RecruitmentVm>> GetRecruitmentsByTitle(string title);
        Task<List<RecruitmentVm>> GetRecruitmentsByLocation(string location);

        Task<List<RecruitmentVm>> GetRecruitmentsByid(int id);
        Task<bool> AddRecruitmentAsync(RecruitmentVm recruitmentVm);
        Task<bool> EditRecruitmentAsync(int id, RecruitmentVm recruitmentVm);
        Task<bool> DeleteRecruitmentAsync(int id);

        Task<Recruitment> GetRecruitmentById(int id);

        Task<bool> UpdateRecruitmentView(int id);
        Task<int> GetTotalRecruitmentsByStatus(int status);
        Task<List<RecruitmentVm>> GetRecruitmentByCompanyName(string company);
        Task<List<RecruitmentVm>> GetRecruitmentByid(int id);
        Task<int> GetTotalViews();
    }
}

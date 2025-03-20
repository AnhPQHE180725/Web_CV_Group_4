using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Interfaces
{
    public interface IRecruitmentService
    {
        Task<List<Recruitment>> GetAllRecruitments();

        Task<List<Recruitment>> GetTop2Recruitments();
        Task<List<RecruitmentVm>> GetRecruitmentsByCompany(int id);
        Task<List<RecruitmentVm>> GetRecruitmentsByCategory(int id);
    }
}

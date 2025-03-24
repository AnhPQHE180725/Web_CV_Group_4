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
        Task<List<Recruitment>> GetRecruitmentsByCompanyName(string company);
        Task<List<Recruitment>> GetRecruitmentsByTitle(string title);
        Task<List<Recruitment>> GetRecruitmentsByLocation(string location);
    }
}

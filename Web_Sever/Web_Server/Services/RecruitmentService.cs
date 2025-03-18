using Web_Server.Interfaces;
using Web_Server.Models;

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

        public async Task<List<Recruitment>> GetRecruitmentsByCategory(int id)
        {
           return await _repository.GetRecruitmentsByCategory(id);
        }

        public async Task<List<Recruitment>> GetRecruitmentsByCompany(int id)
        {
            return await _repository.GetRecruitmentsByCompany(id);
        }

        public async Task<List<Recruitment>> GetTop2Recruitments()
        {
            return await _repository.GetTop2Recruitments();
        }
    }
}

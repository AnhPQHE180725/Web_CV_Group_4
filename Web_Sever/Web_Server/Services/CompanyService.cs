using Web_Server.Interfaces;
using Web_Server.Models;

namespace Web_Server.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<List<Company>> GetAllCompanies()
        {
            return await _companyRepository.GetAllCompanies();
        }

        public async Task<List<Company>> GetTop4Companies()
        {
            return await _companyRepository.GetTop4Companies();
        }
        public async Task<List<Company>> GetCompaniesByName(string name)
        {
            return await _companyRepository.GetCompaniesByName(name);
        }
    }
}

using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllCompanies();
        Task<List<Company>> GetTop4Companies();
        Task<List<Company>> GetCompaniesByName(string companyName);
        Task<Company> GetByIdAsync(int id);
        Task<Company> CreateAsync(Company company);
        Task<Company> UpdateAsync(Company company);
        Task<bool> DeleteAsync(int id);
        Task<List<Company>> GetCompaniesByUserIdAsync(int userId);
    }
}

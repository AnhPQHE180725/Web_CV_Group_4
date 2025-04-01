using Microsoft.AspNetCore.Http;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Interfaces
{
    public interface ICompanyService
    {
        Task<List<Company>> GetAllCompanies();
        Task<List<Company>> GetTop4Companies();
        Task<List<Company>> GetCompaniesByName(string companyName);
        Task<Company> GetCompanyByIdAsync(int id);
        Task<Company> CreateCompanyAsync(CompanyCreateModel companyModel);
  
        Task<bool> DeleteCompanyAsync(int id);
        Task<List<Company>> GetUserCompaniesAsync();
        Task<string?> GetLogoFilePathAsync(string logoFileName);
        Task<Company> UpdateCompanyAsync(CompanyUpdateModel companyModel);
    }
}
using Microsoft.AspNetCore.Http;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyService(ICompanyRepository companyRepository, IHttpContextAccessor httpContextAccessor)
        {
            _companyRepository = companyRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<int> GetCurrentUserIdAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                throw new UnauthorizedAccessException("Invalid user ID in token");

            return userId;
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
        
        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _companyRepository.GetByIdAsync(id);
        }
        
        public async Task<Company> CreateCompanyAsync(CompanyCreateModel companyModel)
        {
            var userId = await GetCurrentUserIdAsync();
            
            var company = new Company
            {
                Name = companyModel.Name,
                Description = companyModel.Description,
                Address = companyModel.Address,
                Email = companyModel.Email,
                PhoneNumber = companyModel.PhoneNumber,
                Logo = companyModel.Logo ?? "",
                Status = companyModel.Status,
                UserId = userId
            };
            
            return await _companyRepository.CreateAsync(company);
        }
        
        public async Task<Company> UpdateCompanyAsync(CompanyUpdateModel companyModel)
        {
            var existingCompany = await _companyRepository.GetByIdAsync(companyModel.Id);
            
            if (existingCompany == null)
                return null;
                
            existingCompany.Name = companyModel.Name;
            existingCompany.Description = companyModel.Description;
            existingCompany.Address = companyModel.Address;
            existingCompany.Email = companyModel.Email;
            existingCompany.PhoneNumber = companyModel.PhoneNumber;
            existingCompany.Logo = companyModel.Logo ?? existingCompany.Logo;
            existingCompany.Status = companyModel.Status;
            
            return await _companyRepository.UpdateAsync(existingCompany);
        }
        
        public async Task<bool> DeleteCompanyAsync(int id)
        {
            return await _companyRepository.DeleteAsync(id);
        }

        public async Task<List<Company>> GetUserCompaniesAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            return await _companyRepository.GetCompaniesByUserIdAsync(userId);
        }
    }
}

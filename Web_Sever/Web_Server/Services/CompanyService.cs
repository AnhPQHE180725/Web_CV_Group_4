using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _environment;
        private const string LOGO_FOLDER = "CompanyLogos";

        public CompanyService(
            ICompanyRepository companyRepository, 
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment environment)
        {
            _companyRepository = companyRepository;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
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

        private async Task<string> SaveLogoFileAsync(IFormFile logoFile)
        {
            if (logoFile == null || logoFile.Length == 0)
                throw new ArgumentException("Logo file is required");

            if (!logoFile.ContentType.StartsWith("image/"))
                throw new ArgumentException("Only image files are allowed");

            if (string.IsNullOrEmpty(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            // Create wwwroot folder if it doesn't exist
            if (!Directory.Exists(_environment.WebRootPath))
            {
                Directory.CreateDirectory(_environment.WebRootPath);
            }

            // Create CompanyLogos folder if it doesn't exist
            var logoFolderPath = Path.Combine(_environment.WebRootPath, LOGO_FOLDER);
            if (!Directory.Exists(logoFolderPath))
                Directory.CreateDirectory(logoFolderPath);

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}_{logoFile.FileName}";
            var filePath = Path.Combine(logoFolderPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await logoFile.CopyToAsync(stream);
            }

            return fileName;
        }

        private async Task DeleteLogoFileAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            if (string.IsNullOrEmpty(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var filePath = Path.Combine(_environment.WebRootPath, LOGO_FOLDER, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
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
        
        public async Task<Company> CreateCompanyAsync(CompanyCreateModel companyModel, IFormFile logoFile)
        {
            var userId = await GetCurrentUserIdAsync();
            var logoFileName = await SaveLogoFileAsync(logoFile);
            
            var company = new Company
            {
                Name = companyModel.Name,
                Description = companyModel.Description,
                Address = companyModel.Address,
                Email = companyModel.Email,
                PhoneNumber = companyModel.PhoneNumber,
                Logo = logoFileName,
                Status = companyModel.Status,
                UserId = userId
            };
            
            return await _companyRepository.CreateAsync(company);
        }
        
        public async Task<Company> UpdateCompanyAsync(CompanyUpdateModel companyModel, IFormFile logoFile)
        {
            var existingCompany = await _companyRepository.GetByIdAsync(companyModel.Id);
            
            if (existingCompany == null)
                return null;

            // Handle logo file
            if (logoFile != null && logoFile.Length > 0)
            {
                // Delete old logo file
                await DeleteLogoFileAsync(existingCompany.Logo);
                // Save new logo file
                existingCompany.Logo = await SaveLogoFileAsync(logoFile);
            }
                
            existingCompany.Name = companyModel.Name;
            existingCompany.Description = companyModel.Description;
            existingCompany.Address = companyModel.Address;
            existingCompany.Email = companyModel.Email;
            existingCompany.PhoneNumber = companyModel.PhoneNumber;
            existingCompany.Status = companyModel.Status;
            
            return await _companyRepository.UpdateAsync(existingCompany);
        }
        
        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
                return false;

            // Delete logo file
            await DeleteLogoFileAsync(company.Logo);
            
            return await _companyRepository.DeleteAsync(id);
        }

        public async Task<List<Company>> GetUserCompaniesAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            return await _companyRepository.GetCompaniesByUserIdAsync(userId);
        }

        public async Task<string?> GetLogoFilePathAsync(string logoFileName)
        {
            if (string.IsNullOrEmpty(logoFileName))
                return null;

            if (string.IsNullOrEmpty(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var filePath = Path.Combine(_environment.WebRootPath, LOGO_FOLDER, logoFileName);
            return File.Exists(filePath) ? filePath : null;
        }

        public async Task<Company> GetCompanyProfileAsync()
        {
            return await _companyRepository.GetCompanyProfileAsync();
        }
    }
}

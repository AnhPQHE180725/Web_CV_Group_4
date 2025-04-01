﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
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

        private async Task<string> SaveLogoFromUrlAsync(string logoUrl)
        {
            if (string.IsNullOrEmpty(logoUrl))
                throw new ArgumentException("Logo URL is required");

            // Validate URL
            if (!Uri.TryCreate(logoUrl, UriKind.Absolute, out Uri uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                throw new ArgumentException("Invalid logo URL format");

            // Validate file extension
            string fileExtension = Path.GetExtension(logoUrl).ToLower();
            if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                throw new ArgumentException("Only .jpg, .jpeg and .png formats are supported");

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
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(logoFolderPath, fileName);

            try
            {
                // Download the image
                using (WebClient client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(new Uri(logoUrl), filePath);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to download logo: {ex.Message}");
            }
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

        public async Task<Company> CreateCompanyAsync(CompanyCreateModel companyModel)
        {
            var userId = await GetCurrentUserIdAsync();

            // Validate and save logo from URL
            if (string.IsNullOrEmpty(companyModel.Logo))
                throw new ArgumentException("Logo URL is required");

            string logoFileName = await SaveLogoFromUrlAsync(companyModel.Logo);

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

        public async Task<Company> UpdateCompanyAsync(CompanyUpdateModel companyModel)
        {
            var existingCompany = await _companyRepository.GetByIdAsync(companyModel.Id);

            if (existingCompany == null)
                return null;

            // Update logo if new URL is provided
            if (!string.IsNullOrEmpty(companyModel.Logo))
            {
                // Delete old logo file
                await DeleteLogoFileAsync(existingCompany.Logo);
                // Save new logo from URL
                existingCompany.Logo = await SaveLogoFromUrlAsync(companyModel.Logo);
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
    }
}
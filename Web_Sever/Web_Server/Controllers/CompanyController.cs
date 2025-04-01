using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web_Server.Interfaces;
using Web_Server.ViewModels;

namespace Web_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("get-all-companies")]
        public async Task<IActionResult> GetAllCompanies()
        {
            return Ok(await _companyService.GetAllCompanies());
        }

        [HttpGet("get-top-companies")]
        public async Task<IActionResult> GetTopCompanies()
        {
            return Ok(await _companyService.GetTop4Companies());
        }

        [HttpGet("get-companies-by-name/{company}")]
        public async Task<IActionResult> GetCompaniesByName(string company)
        {
            return Ok(await _companyService.GetCompaniesByName(company));
        }

        [HttpGet("get-company/{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
                return NotFound();

            return Ok(company);
        }

        [HttpGet("my-companies")]
        [Authorize]
        public async Task<IActionResult> GetUserCompanies()
        {
            try
            {
                var companies = await _companyService.GetUserCompaniesAsync();
                return Ok(companies);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching your companies" });
            }
        }

        [HttpPost("create-company")]
        [Authorize]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateModel companyModel, IFormFile logoFile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdCompany = await _companyService.CreateCompanyAsync(companyModel, logoFile);
                return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompany.Id }, createdCompany);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the company" });
            }
        }

        [HttpPut("update-company")]
        [Authorize]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyUpdateModel companyModel, IFormFile logoFile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedCompany = await _companyService.UpdateCompanyAsync(companyModel, logoFile);
                if (updatedCompany == null)
                    return NotFound();

                return Ok(updatedCompany);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the company" });
            }
        }

        [HttpDelete("delete-company/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                var result = await _companyService.DeleteCompanyAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the company" });
            }
        }

        [HttpGet("logo/{logoFileName}")]
        public async Task<IActionResult> GetCompanyLogo(string logoFileName)
        {
            var filePath = await _companyService.GetLogoFilePathAsync(logoFileName);
            if (filePath == null)
                return NotFound("Logo file not found.");

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "image/jpeg");
        }
    }
}
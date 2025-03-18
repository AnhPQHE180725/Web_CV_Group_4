using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Server.Interfaces;

namespace Web_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private  readonly ICompanyService _companyService;

        public CompanyController (ICompanyService companyService)
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

    }
}

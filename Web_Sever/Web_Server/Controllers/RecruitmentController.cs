using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Web_Server.Interfaces;

namespace Web_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruitmentController : ControllerBase
    {
        private readonly IRecruitmentService _recruitmentService;

        public RecruitmentController(IRecruitmentService recruitmentService)
        {
            _recruitmentService = recruitmentService;
        }
        [HttpGet("get-all-recruitments")]

        public async Task<IActionResult> GetAllRecruitments()
        {
            return Ok(await _recruitmentService.GetAllRecruitments());
        }

        [HttpGet("get-top-recruitments")]

        public async Task<IActionResult> GetTopRecruitments()
        {
            return Ok(await _recruitmentService.GetTop2Recruitments());
        }

        [HttpGet("get-recruitments-by-company-id/{id}")]

        public async Task<IActionResult> GetRecruitmentsByCompanyId(int id)
        {
            return Ok(await _recruitmentService.GetRecruitmentsByCompany(id));
        }
        [HttpGet("get-recruitments-by-category-id/{id}")]

        public async Task<IActionResult> GetRecruitmentsByCategoryId(int id)
        {
            return Ok(await _recruitmentService.GetRecruitmentsByCategory(id));
        }

        [HttpGet("get-recruitments-by-company-name/{company}")]
        public async Task<IActionResult> GetRecruitmentsByCompanyName(string company)
        {
            return Ok(await _recruitmentService.GetRecruitmentsByCompanyName(company));
        }
        [HttpGet("get-recruitments-by-title/{title}")]
        public async Task<IActionResult> GetRecruitmentsByTitle(string title)
        {
            return Ok(await _recruitmentService.GetRecruitmentsByTitle(title));
        }
        [HttpGet("get-recruitments-by-location/{location}")]
        public async Task<IActionResult> GetRecruitmentsByLocation(string location)
        {
            return Ok(await _recruitmentService.GetRecruitmentsByLocation(location));
        }
    }
}

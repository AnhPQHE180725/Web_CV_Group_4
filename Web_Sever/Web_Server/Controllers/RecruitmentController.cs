using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Server.Interfaces;
using Web_Server.ViewModels;

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
        [HttpPost("add-recruitment")]
        public async Task<IActionResult> AddRecruitment([FromBody] RecruitmentVm recruitmentVm)
        {
            var result = await _recruitmentService.AddRecruitmentAsync(recruitmentVm);
            if (!result)
            {
                return BadRequest("Failed to add recruitment");
            }
            return Ok("Recruitment added successfully");
        }

        [HttpPut("edit-recruitment/{id}")]
        public async Task<IActionResult> EditRecruitment(int id, [FromBody] RecruitmentVm recruitmentVm)
        {
            var result = await _recruitmentService.EditRecruitmentAsync(id, recruitmentVm);
            if (!result)
            {
                return NotFound("Recruitment not found");
            }
            return Ok("Recruitment updated successfully");
        }

        [HttpDelete("delete-recruitment/{id}")]
        public async Task<IActionResult> DeleteRecruitment(int id)
        {
            var result = await _recruitmentService.DeleteRecruitmentAsync(id);
            if (!result)
            {
                return NotFound("Recruitment not found");
            }
            return Ok("Recruitment deleted successfully");
        }
    }
}

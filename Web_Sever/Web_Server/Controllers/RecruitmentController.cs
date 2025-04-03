using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Recruiter")]
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
        [Authorize(Roles = "Recruiter")]
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
        [Authorize(Roles = "Recruiter")]
        [HttpDelete("delete-recruitment/{id}")]
        public async Task<IActionResult> DeleteRecruitment(int id)
        {
            var recruitment = await _recruitmentService.GetRecruitmentByid(id);

            if (recruitment == null)
            {
                return NotFound(new { message = "Recruitment not found" });
            }

            await _recruitmentService.DeleteRecruitmentAsync(id);

            // ✅ Thêm phản hồi rõ ràng sau khi xóa thành công
            return Ok(new { message = "Recruitment deleted successfully" });
        }

        [HttpGet("get-recruitment-by-id/{id}")]

        public async Task<IActionResult> GetRecruitmentById(int id)
        {
            return Ok(await _recruitmentService.GetRecruitmentById(id));
        }

        [HttpPut("increment-view/{id}")]
        public async Task<IActionResult> IncrementView(int id)
        {
            var success = await _recruitmentService.UpdateRecruitmentView(id);
            if (!success)
            {
                return NotFound("Recruitment not found");
            }

            return Ok("View incremented");
        }

        [HttpPut("update-view/{id}")]
        public async Task<IActionResult> UpdateView(int id)
        {
            var success = await _recruitmentService.UpdateRecruitmentView(id);
            if (!success)
            {
                return NotFound(new { message = "Recruitment not found" });
            }

            return Ok(new { message = "View updated successfully" });
        }

        [HttpGet("recruitments/count")]
        public async Task<IActionResult> GetTotalRecruitments(int status)
        {
            var totalRecruitments = await _recruitmentService.GetTotalRecruitmentsByStatus(status);
            return Ok(totalRecruitments);
        }
        [HttpGet("get-recruitment-by-company-name/{company}")]
        public async Task<IActionResult> GetRecruitmentByCompanyName(string company)
        {
            return Ok(await _recruitmentService.GetRecruitmentByCompanyName(company));
        }

    }
}
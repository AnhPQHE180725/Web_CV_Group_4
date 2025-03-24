using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplyPostController : ControllerBase
    {
        private readonly IApplyPostService _applyPostService;

        public ApplyPostController(IApplyPostService applyPostService)
        {
            _applyPostService = applyPostService;
        }

        [HttpPost("apply-with-new-cv")]
        public async Task<IActionResult> ApplyWithNewCV([FromForm] ApplyWithNewCVVm applyVm)
        {
            try
            {
                var application = await _applyPostService.ApplyWithNewCVAsync(applyVm);
                return Ok(new { 
                    message = "Application submitted successfully with new CV",
                    application = application
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex });
            }
        }

        [HttpPost("apply-with-existing-cv")]
        public async Task<IActionResult> ApplyWithExistingCV([FromBody] ApplyWithExistingCVVm applyVm)
        {
            try
            {
                var application = await _applyPostService.ApplyWithExistingCVAsync(applyVm);
                return Ok(new { 
                    message = "Application submitted successfully with existing CV",
                    application = application
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request" });
            }
        }

        [HttpGet("user-applications")]
        public async Task<IActionResult> GetUserApplications()
        {
            try
            {
                var userIdClaim = User.FindFirst("id");
                if (userIdClaim == null)
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(userIdClaim.Value, out int userId))
                    return Unauthorized(new { message = "Invalid user ID in token" });

                var applications = await _applyPostService.GetApplicationsByUserIdAsync(userId);
                return Ok(applications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching applications" });
            }
        }

        [HttpGet("post-applications/{postId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPostApplications(int postId)
        {
            try
            {
                var applications = await _applyPostService.GetApplicationsByPostIdAsync(postId);
                return Ok(applications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching applications" });
            }
        }

        [HttpPut("status/{applicationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateApplicationStatus(int applicationId, [FromBody] int status)
        {
            try
            {
                var result = await _applyPostService.UpdateApplicationStatusAsync(applicationId, status);
                if (result)
                    return Ok(new { message = "Application status updated successfully" });
                return NotFound(new { message = "Application not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating application status" });
            }
        }
    }
}
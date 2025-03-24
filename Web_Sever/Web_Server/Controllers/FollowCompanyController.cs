using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Server.Interfaces;

namespace Web_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FollowCompanyController : ControllerBase
    {
        private readonly IFollowCompanyService _followCompanyService;

        public FollowCompanyController(IFollowCompanyService followCompanyService)
        {
            _followCompanyService = followCompanyService;
        }

        [HttpPost("toggle/{companyId}")]
        public async Task<IActionResult> ToggleFollowCompany(int companyId)
        {
            try
            {
                var (success, message) = await _followCompanyService.ToggleFollowCompanyAsync(companyId);
                if (!success)
                    return BadRequest(new { message });
                return Ok(new { message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request" });
            }
        }

        [HttpGet("user-follows")]
        public async Task<IActionResult> GetUserFollowCompanies()
        {
            try
            {
                var follows = await _followCompanyService.GetUserFollowCompaniesAsync();
                return Ok(follows);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching follows" });
            }
        }

        [HttpGet("check/{companyId}")]
        public async Task<IActionResult> CheckFollowingCompany(int companyId)
        {
            try
            {
                var isFollowing = await _followCompanyService.IsFollowingCompanyAsync(companyId);
                return Ok(new { isFollowing });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while checking follow status" });
            }
        }

        [HttpGet("followed-ids")]
        public async Task<IActionResult> GetFollowedCompanyIds()
        {
            try
            {
                var followedIds = await _followCompanyService.GetFollowedCompanyIdsAsync();
                return Ok(new { followedIds });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching followed IDs" });
            }
        }
    }
}

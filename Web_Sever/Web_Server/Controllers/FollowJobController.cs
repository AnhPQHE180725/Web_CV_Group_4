using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Server.Interfaces;

namespace Web_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FollowJobController : ControllerBase
    {
        private readonly IFollowJobService _followJobService;

        public FollowJobController(IFollowJobService followJobService)
        {
            _followJobService = followJobService;
        }

        [HttpPost("toggle/{recruitmentId}")]
        public async Task<IActionResult> ToggleFollowJob(int recruitmentId)
        {
            try
            {
                var (success, message) = await _followJobService.ToggleFollowJobAsync(recruitmentId);
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
        public async Task<IActionResult> GetUserFollowJobs()
        {
            try
            {
                var follows = await _followJobService.GetUserFollowJobsAsync();
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

        [HttpGet("followed-ids")]
        public async Task<IActionResult> GetFollowedJobIds()
        {
            try
            {
                var followedIds = await _followJobService.GetFollowedJobIdsAsync();
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

        [HttpGet("check/{recruitmentId}")]
        public async Task<IActionResult> CheckFollowingJob(int recruitmentId)
        {
            try
            {
                var isFollowing = await _followJobService.IsFollowingJobAsync(recruitmentId);
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
    }
}

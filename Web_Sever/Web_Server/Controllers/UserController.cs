using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Server.Interfaces;

namespace Web_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize(Roles = "Recruiter")]
        [HttpGet("get-user-by-post-id/{id}")]

        public async Task<IActionResult> GetUserByPostId(int id)
        {
            return Ok(await _userService.GetCandidateByPostId(id));
        }
        [Authorize]
        [HttpGet("get-cv-by-user-id/{id}")]

        public async Task<IActionResult> GetCVByUserId(int id)
        {
            return Ok(await _userService.GetCVByUserId(id));
        }
        [Authorize(Roles = "Recruiter")]
        [HttpPut("apply-cv/{id}")]
        public async Task<IActionResult> ApplyCV(int id)
        {
            return Ok(await _userService.ApplyCV(id));
        }
        [Authorize(Roles = "Recruiter")]
        [HttpPut("reject-cv/{id}")]
        public async Task<IActionResult> RejectCV(int id)
        {
            return Ok(await _userService.RejectCV(id));
        }

    }
}

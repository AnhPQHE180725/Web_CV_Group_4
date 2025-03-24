using Microsoft.AspNetCore.Http;
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

        [HttpGet("get-user-by-post-id/{id}")]

        public async Task<IActionResult> GetUserByPostId(int id)
        { 
        return Ok(await _userService.GetCandidateByPostId(id));
}
        [HttpGet("get-cv-by-user-id/{id}")]

        public async Task<IActionResult> GetCVByUserId(int id)
        {
            return Ok(await _userService.GetCVByUserId(id));
        }

    }
}

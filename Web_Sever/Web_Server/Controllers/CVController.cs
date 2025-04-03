using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Server.Interfaces;
using Web_Server.Models;

namespace Web_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CVController : ControllerBase
    {
        private readonly ICVService _cvService;

        public CVController(ICVService cvService)
        {
            _cvService = cvService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CV>> GetCVById(int id)
        {
            var cv = await _cvService.GetCVByIdAsync(id);
            if (cv == null)
                return NotFound();

            return Ok(cv);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CV>> GetCVByUserId(int userId)
        {
            var cv = await _cvService.GetCVByUserIdAsync(userId);
            if (cv == null)
                return NotFound();

            return Ok(cv);
        }
        [Authorize]
        [HttpPost("upload")]
        public async Task<ActionResult<CV>> UploadCV(IFormFile file)
        {
            try
            {
                var cv = await _cvService.UploadCVAsync(file);
                return Ok(cv);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCV(int id)
        {
            var result = await _cvService.DeleteCVAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("view/{userId}")]
        public async Task<IActionResult> ViewCV(int userId)
        {
            var filePath = await _cvService.GetCVFilePathByUserIdAsync(userId);
            if (filePath == null)
                return NotFound("CV file not found.");

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/pdf");
        }

    }
}
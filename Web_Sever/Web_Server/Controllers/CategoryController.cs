using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Server.Interfaces;
using Web_Server.Services;

namespace Web_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("get-all-categories")]

        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await categoryService.GetAllCategories());
        }

        [HttpGet("get-top-categories")]

        public async Task<IActionResult> GetTopCategories()
        {
            return Ok(await categoryService.Get5TopCategories());
        }
    }
}

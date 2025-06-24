using ECommercePlatform.Api.Common;
using ECommercePlatform.Logic.Services.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odai.Domain.Entities;
using Odai.Shared.Models;

namespace Odai.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CategoryController : BaseController<Category, ICategoryService>
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService) : base(categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("AddEdit")]
        public async Task<IActionResult>AddEdit([FromForm]CategoryModel model)
        {
            var category = await _categoryService.AddEdit(model);
            if (category != null)
            {
                return Ok(category);
            }
            return BadRequest();
        }
        [HttpGet("GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _service.GetCategoryById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}

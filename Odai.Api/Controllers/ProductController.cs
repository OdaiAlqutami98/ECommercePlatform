using ECommercePlatform.Api.Common;
using ECommercePlatform.Logic.Services.Product;
using Microsoft.AspNetCore.Mvc;
using Odai.Domain.Entities;
using Odai.Domain.Enums;
using Odai.Shared.Models;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController<Product, IProductService>
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService) : base(productService)
        {
            _productService = productService;
        }
        [HttpPost("AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm]ProductModel model)
        {
            var result = await _productService.AddEdit(model);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet("GetProductBySort")]
        public async Task<IActionResult> GetProductBySort(int? categoryId, ProductSortOption option)
        {
            var result = await _productService.GetProductBySort(categoryId, option);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _service.GetProductById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}

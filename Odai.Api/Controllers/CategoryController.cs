using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using Odai.Api.Extension;
using Odai.Domain.Entities;
using Odai.Domain.Enums;
using Odai.Logic.Manager;
using Odai.Shared.Auth;
using Odai.Shared.Models;
using Odai.Shared.Table;

namespace Odai.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly CategoryManager _categoryManager;
        private readonly ProductManager _productManager;

        public CategoryController(CategoryManager categoryManager, ProductManager productManager)
        {
            _categoryManager = categoryManager;
            _productManager = productManager;
        }
        [HttpGet("GetCategory")]
        public async Task<TableResponse<Category>> GetCategory(int pageIndex, int pageSize)
        {
            var query = _categoryManager.GetAll().Include(c=>c.Products);
            var length = query.Count();

            var category = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new TableResponse<Category>
            {
                RecordsTotal = length,
                Data = category
            };
        }
        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var category = await _categoryManager.GetById(Id);
            if (category != null)
            {
                return Ok(category);
            }
            return BadRequest(false);

        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult> AddEdit(CategoryModel model)
        {
            UpladFileModel filePath = new UpladFileModel();
            if (model.ImagePath != null)
            {

                filePath = await Extension.Extension.UploadFile(model.ImagePath);
            }
            if (model.Id == null)
            {
                Category category = new Category();
                category.Name = model.Name;
                category.FilePath = filePath.FileName;
                category.ContentType = filePath.ContentType;
                category.CreatonDate = DateTime.Now;
                category.LastUpdateDate = DateTime.Now;
                await _categoryManager.Add(category);
                await _categoryManager.SaveChangesAsync();
                return Ok(new Response<bool>(true,"Category Added Success"));
            }
            else
            {
                var category = await _categoryManager.Get(c => c.Id == model.Id).FirstOrDefaultAsync();
                if (category != null)
                {
                    category.Name = model.Name;
                    category.FilePath = filePath.FileName;
                    category.ContentType = filePath.ContentType;
                    category.LastUpdateDate = DateTime.Now;
                    _categoryManager.Update(category);
                    await _categoryManager.SaveChangesAsync();
                    return Ok(new Response<bool>(true));
                }
                else
                {
                    return NoContent();
                }
            }
        }
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var category = await _categoryManager.Get(c => c.Id == Id).FirstOrDefaultAsync();
            var hasProducts = await _productManager.Get(p => p.CategoryId == Id).AnyAsync();
            if (hasProducts)
            {
                return BadRequest(new Response<bool>
                {
                    Succeeded = false,
                    Message = "Cannot delete this category because it is associated with existing products."
                });
            }

            if (category != null)
            {
                await _categoryManager.Delete(category);
                await _categoryManager.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Category deleted successfully." });
            }
            return NotFound(new Response<bool> { Succeeded = false, Message = "Category not found." });
        }
    }
}

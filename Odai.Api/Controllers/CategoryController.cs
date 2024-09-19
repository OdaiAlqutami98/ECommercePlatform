using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using Odai.Domain;
using Odai.Logic.Manager;
using Odai.Shared;
using Odai.Shared.Auth;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(CategoryManager _categoryManager) : ControllerBase
    {
     //   private readonly CategoryManager _categoryManager;
        //public CategoryController(CategoryManager categoryManager)
        //{
        //    _categoryManager = categoryManager;
        //}

        [HttpGet]
        public async Task<IActionResult>GetCategory()
        {
            var category = await _categoryManager.GetAll().Include(p=>p.Products).ToListAsync();
                 //.Select(c => new
                 //{
                 //  c.Name,
                 //    ProductName = c.Products.ToArray(),
                 //}).ToListAsync();//.Include(c=>c.Products).ToListAsync();
            if (category != null)
            {
                return Ok(category);
            }
            return BadRequest(false);
          
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _categoryManager.GetById(id);
            if (product != null)
            {
                return Ok(product);
            }
            return BadRequest(false);

        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult>AddEdit(CategoryModel model)
        {
            if (model.Id==null)
            {
                Category category=new Category();
                category.Name = model.Name;
                category.CreatonDate = DateTime.Now;
                category.LastUpdateDate= DateTime.Now;
                await _categoryManager.Add(category);
                await _categoryManager.SaveChangesAsync();
                return Ok(new Response<bool>());
            }
            else
            {
                var category=await _categoryManager.Get(c=>c.Id==model.Id).FirstOrDefaultAsync();
                if (category!=null)
                {
                    category.Name = model.Name;
                    category.LastUpdateDate = DateTime.Now;
                    _categoryManager.Update(category);
                    await _categoryManager.SaveChangesAsync();
                    return Ok(new Response<bool>());    
                }
                else
                {
                    return NoContent();
                }
            }
        }
        [HttpDelete]
        [Route("Delete/{Id}")]
        public async Task<IActionResult>Delete(int Id)
        {
            var category= await _categoryManager.Get(c => c.Id == Id).FirstOrDefaultAsync();
            if (category!=null)
            {
                await _categoryManager.Delete(category);
              await  _categoryManager.SaveChangesAsync();
                return Ok(new Response<bool>());
            }
            return NotFound(new Response<bool>("Not Found Category"));
        }
    }
}

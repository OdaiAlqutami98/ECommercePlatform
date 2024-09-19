using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odai.Domain;
using Odai.Logic.Manager;
using Odai.Shared;
using Odai.Shared.Auth;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(ProductManager _productManager) : ControllerBase
    {
        //private readonly ProductManager _productManager;
        //public ProductController(ProductManager productManager)
        //{
        //    _productManager = productManager;
        //}
        [HttpGet]
        [Route("GetProduct")]
        public async Task<IActionResult> GetProduct()
        {
            var product=await _productManager.GetAll().Include(c=>c.Category).ToListAsync();
                //.Select(p => new
                //{
                //    p.Name,
                //    p.Description,
                //    p.Price,
                //    CategoryName = p.Category.Name
                //}).ToListAsync();
            if (product != null)
            {
                return Ok(product);
            }
            return BadRequest(false);
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult>GetById(int id)
        {
            var product=await _productManager.GetById(id);
            if (product != null)
            {
                return Ok(product);
            }
            return BadRequest(false);
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult>AddEdit(ProductModel model)
        {
            if (model.Id == null)
            {
                Product product=new Product();
                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                product.Favorite = model.Favorite;
                product.CreatonDate=DateTime.Now;
                await _productManager.Add(product);
                await _productManager.SaveChangesAsync();
                return Ok(new Response<bool>());
            }
            else
            {
                var product = await _productManager.Get(p=>p.Id==model.Id).FirstOrDefaultAsync();
                if (product != null)
                {
                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.Favorite = model.Favorite;
                    product.Price= model.Price;
                    product.CategoryId = model.CategoryId;
                    product.LastUpdateDate= DateTime.Now;
                     _productManager.Update(product);
                    await _productManager.SaveChangesAsync();
                    return Ok(new Response<bool>());
                }
                else
                {
                    return NoContent();
                }
            }
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult>Delete(int id)
        {
            var product=await _productManager.Get(p=>p.Id==id).FirstOrDefaultAsync();
            if (product != null)
            {
                await _productManager.Delete(product);
                await _productManager.SaveChangesAsync();
                return Ok(new Response<bool>());
            }
            return BadRequest(new Response<bool>("Product not found"));
        }
    }
}

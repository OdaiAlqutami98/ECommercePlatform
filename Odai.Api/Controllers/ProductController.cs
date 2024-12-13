using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odai.Api.Extension;
using Odai.Domain;
using Odai.Domain.Enums;
using Odai.Logic.Manager;
using Odai.Shared;
using Odai.Shared.Auth;
using Odai.Shared.Table;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductManager _productManager;
        public ProductController(ProductManager productManager)
        {
            _productManager = productManager;
        }
        [HttpGet("GetProduct")]
        public async Task<TableResponse<Product>> GetProduct(int pageIndex, int pageSize)
        {
            var query = _productManager
                .GetAll()
                .Include(p => p.Ratings)
                .Include(p => p.Comments);
            var length = query.Count();

            var product = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new TableResponse<Product>
            {
                RecordsTotal = length,
                Data = product,
            };
        }
        [HttpGet("GetById")]
        public async Task<IActionResult>GetById(int id)
        {
            var product=await _productManager.GetById(id);
            if (product != null)
            {
                return Ok(product);
            }
            return BadRequest(false);
        }
        [HttpPost("AddEdit")]
        public async Task<IActionResult>AddEdit(ProductModel model)
        {
            UpladFileModel filePath = new UpladFileModel();
            if (model.ImagePath != null)
            {

                filePath = await Extension.Extension.UploadFile(model.ImagePath);
            }
            if (model.Id == null)
            {
                Product product=new Product();
                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                product.Favorite = model.Favorite;
                product.Stock=model.Stock;
                product.FilePath = filePath.FileName;
                product.ContentType = filePath.ContentType;
                product.Status = (ProductStatus)model.Status;
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
                    product.Stock = model.Stock;
                    product.Status = (ProductStatus)model.Status;
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
        [HttpDelete("Delete")]
        public async Task<IActionResult>Delete(int id)
        {
            var product = await _productManager.Get(p => p.Id == id).FirstOrDefaultAsync();
            if (product != null)
            {
                await _productManager.Delete(product);
                await _productManager.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Product deleted successfully." });
            }
            return BadRequest(new Response<bool>("Product not found"));
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Odai.Domain;
using Odai.Logic.Common.Interface;
using Odai.Logic.Manager;
using Odai.Shared;
using Odai.Shared.Auth;
using Odai.Shared.Table;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly BasketManager _basketManager;
        public BasketController(BasketManager basketManager)
        {
            _basketManager = basketManager;   
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<TableResponse<Basket>>GetAll(int pageIndex,int pageSize)
        {
            var query = _basketManager
                .GetAll()
                .Include(b => b.BasketItems);
            var length = query.Count();
            var basket = await query
                .Skip(pageIndex*pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new TableResponse<Basket>
            {
                RecordsTotal = length,
                Data = basket
            };
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult>GetById(int Id)
        {
            var basket = await _basketManager.GetById(Id);    
            if (basket != null)
            {
                return Ok(basket);
            }
            return BadRequest(false);
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult>AddEdit(BasketModel model)
        {
           
            if (model.Id == null)
            {
                Basket basket = new Basket();
                basket.BasketItems = model.BasketItems?.Select(item => new BasketItem
                {
                    UnitPrice=item.UnitPrice,
                    BasketId=item.BasketId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                }).ToList();
                basket.UserId = model.UserId; 
                basket.CreatonDate= DateTime.Now;
                basket.CreatedBy = model.UserId;
                await _basketManager.Add(basket);
                await _basketManager.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Item added To basket successfully.", Data = true });
            }
            else
            {
                var basket = await _basketManager.Get(b=>b.Id==model.Id).Include(p=>p.BasketItems).FirstOrDefaultAsync();
                if (basket != null)
                {
                    basket.BasketItems?.Clear();  // Clear existing items
                    basket.BasketItems = model.BasketItems.Select(item => new BasketItem
                    {
                        UnitPrice = item.UnitPrice,
                        BasketId = item.BasketId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    }).ToList();
                    basket.UserId = model.UserId;
                    basket.LastUpdateDate = DateTime.Now;
                    basket.LastUpdateBy = model.UserId;
                    _basketManager.Update(basket);
                   await _basketManager.SaveChangesAsync();
                    return Ok(new Response<bool> { Succeeded = true, Message = "Item updated To basket successfully.", Data = true });
                }
            }
            return BadRequest(false);
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult>Delete(int id)
        {
            var basket = await _basketManager.Get(b => b.Id == id).FirstOrDefaultAsync();
           if (basket != null)
           {
                await _basketManager.Delete(basket);
                await _basketManager.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Basket deleted successfully." });
           }
            return NotFound(new Response<bool>("Not Found Basket"));
        }
    }
}

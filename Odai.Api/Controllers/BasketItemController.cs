using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using Odai.Domain;
using Odai.Logic.Common;
using Odai.Logic.Common.Interface;
using Odai.Logic.Manager;
using Odai.Shared;
using Odai.Shared.Auth;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemController : ControllerBase
    {
        private readonly BasketItemManager _basketItemManager;
        public BasketItemController(BasketItemManager basketItemManager)
        {
            _basketItemManager = basketItemManager;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult>GetAll()
        {
            var basketItem = await _basketItemManager.GetAll()
                .Include(b=>b.Basket)
                .Include(p=>p.Product)
                .ToListAsync();
            if (basketItem != null )
            {
                return Ok(basketItem);
            }
            return BadRequest(new Response<bool>());
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult>GetById(int id)
        {
            var basketItem = await _basketItemManager.GetById(id);
            if (basketItem is not null)
            {
                return Ok(basketItem);
            }
            return BadRequest(new Response<bool>());
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult>AddEdit(BasketItemModel model)
        {
            if (model.Id == null)
            {
                BasketItem basket = new BasketItem();
                basket.BasketId = model.BasketId;
                basket.ProductId = model.ProductId;
                basket.Quantity = model.Quantity;
                basket.UnitPrice=model.UnitPrice;
                basket.UserId = model.UserId;
                basket.CreatonDate = DateTime.Now;
                basket.CreatedBy = model.UserId;
                await _basketItemManager.Add(basket);
                await _basketItemManager.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Item added To Basket successfully.", Data = true });
            }
            else
            {
                var basketItem = await _basketItemManager.Get(bi => bi.Id == model.Id).FirstOrDefaultAsync();
                if (basketItem != null)
                {
                    basketItem.BasketId = model.BasketId;
                    basketItem.ProductId= model.ProductId;
                    basketItem.Quantity= model.Quantity;
                    basketItem.UnitPrice= model.UnitPrice;
                    basketItem.UserId = model.UserId;
                    basketItem.LastUpdateDate = DateTime.Now;
                    basketItem.LastUpdateBy = model.UserId;
                    _basketItemManager.Update(basketItem);
                    await _basketItemManager.SaveChangesAsync();
                    return Ok(new Response<bool> { Succeeded = true, Message = "Item Updated To Basket successfully.", Data=true });
                }
            }
            return BadRequest(new Response<bool>());
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult>Delete(int id)
        {
            var Item = await _basketItemManager.Get(oi => oi.Id == id).Include(oi => oi.Basket).FirstOrDefaultAsync();
            if (Item != null)
            {
                await _basketItemManager.Delete(Item);
                await _basketItemManager.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Item deleted from basket successfully.", Data = true });
            }
            return NotFound("Not Found Basket Item");
        }
    }
}

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
    public class BasketItemController(BasketItemManager _basketItemManager ,IIdentityService _identityService) : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult>GetAll()
        {
            var basketitem=await _basketItemManager.GetAll().Include(b=>b.Basket).ToListAsync();
            if (basketitem is not null )
            {
                return Ok(basketitem);
            }
            return BadRequest(false);
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult>GetById(int id)
        {
            var basketitem=await _basketItemManager.GetById(id);
            if (basketitem is not null)
            {
                return Ok(basketitem);
            }
            return BadRequest(false);
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult>AddEdit(BasketItemModel model)
        {
            var userId = await _identityService.GetUserAsync(model.UserId);
            if (userId is null)
            {
                return Unauthorized("User not found.");
            }
            if (!Guid.TryParse(userId.Id.ToString(), out Guid user))
            {
                return BadRequest("Invalid user ID format.");
            }
            if (model.Id is null)
            {
                BasketItem basket=new BasketItem();
                basket.ProductId = model.ProductId;
                basket.Quantity = model.Quantity;
                basket.CreatonDate = DateTime.Now;
                basket.CreatedBy = user;
                await _basketItemManager.Add(basket);
                await _basketItemManager.SaveChangesAsync();
                return Ok(new Response<bool>("Item added to basket successfully"));
            }
            else
            {
                var basketitem=await _basketItemManager.Get().FirstOrDefaultAsync(b=>b.Id == model.Id);
                if (basketitem is not null)
                {
                    basketitem.ProductId= model.ProductId;
                    basketitem.Quantity= model.Quantity;
                    basketitem.LastUpdateDate = DateTime.Now;
                    basketitem.LastUpdateBy = user;
                    _basketItemManager.Update(basketitem);
                    await _basketItemManager.SaveChangesAsync();
                    return Ok(new Response<bool>("Basket item updated successfully"));
                }
            }
            return BadRequest(new Response<bool>());
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult>Delete(int id)
        {
            var basketitem=await _basketItemManager.GetById(id);
            if (basketitem is not null)
            {
                await _basketItemManager.Delete(basketitem);
                return Ok(new Response<bool>("Item Deleted from basket successfully"));
            }
            return BadRequest(new Response<bool>());
        }
    }
}

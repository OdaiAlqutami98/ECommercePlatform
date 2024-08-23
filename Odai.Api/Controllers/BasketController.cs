using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Odai.Domain;
using Odai.Logic.Common.Interface;
using Odai.Logic.Manager;
using Odai.Shared;
using Odai.Shared.Auth;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController(BasketManager _basketManager, IIdentityService _identityService) : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult>GetAll()
        {
            var basket=await _basketManager.GetAll().Include(b=>b.BasketItems).ToListAsync();
            if (basket is not null)
            {
                return Ok(basket);
            }
            return BadRequest(false);
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult>GetById(int Id)
        {
            var basket = await _basketManager.GetById(Id);    
            if (basket is not null)
            {
                return Ok(basket);
            }
            return BadRequest(false);
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult>AddEdit(BasketModel model)
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
            if (model.Id is not null)
            {
                Basket basket=new Basket();
              //  basket.BasketItems = model.BasketItems.Select(item => new BasketItem
              //  {
              ////   BasketId=item.BasketId,
              //   ProductId=item.ProductId,
              //   Quantity = item.Quantity,
              //  }).ToList();
                basket.UserId = user; 
                basket.CreatonDate= DateTime.Now;
                basket.CreatedBy = user;
                await _basketManager.Add(basket);
                await _basketManager.SaveChangesAsync();
                return Ok(new Response<bool>());
            }
            else
            {
                var basket = await _basketManager.Get().FirstOrDefaultAsync(b => b.Id == model.Id);
                if (basket is not null)
                {
                    //basket.BasketItems = model.BasketItems.Select(item => new BasketItem
                    //{
                    // BasketId = item.BasketId,
                    // ProductId = item.ProductId,
                    // Quantity = item.Quantity,
                    //}).ToList();
                    basket.UserId = user;
                    basket.LastUpdateDate = DateTime.Now;
                    basket.LastUpdateBy = user;
                    _basketManager.Update(basket);
                   await _basketManager.SaveChangesAsync();
                    return Ok(new Response<bool>());
                }
            }
            return BadRequest(false);
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult>Delete(int id)
        {
            var basket=await _basketManager.Get().FirstOrDefaultAsync(b => b.Id == id);
           if (basket is not null)
           {
                await _basketManager.Delete(basket);
                await _basketManager.SaveChangesAsync();
                return Ok(new Response<bool>());
           }
            return BadRequest(new Response<bool>());
        }
    }
}

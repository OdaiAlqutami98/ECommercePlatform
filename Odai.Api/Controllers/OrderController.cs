using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class OrderController(OrderManager _orderManager,IIdentityService _identityService) : ControllerBase
    {
        //private readonly OrderManager _orderManager;
        //public OrderController(OrderManager orderManager)
        //{
        //    _orderManager = orderManager;
        //}
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult>GetAll()
        {
            var order=await _orderManager.GetAll().Include(o=>o.OrderItems).ToListAsync();
            if (order.Any())
            {
                return  Ok(order);
            }
            return BadRequest(false);
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult>GetById(int id)
        {
            var order = await _orderManager.GetById(id);
            if (order!=null)
            {
                return Ok(order);
            }
            return BadRequest(false);
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult>AddEdit(OrderModel model)
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
            if (model.Id==null)
            {
                Order order=new Order();
                order.TotalPrice = model.TotalPrice;
                order.Status = model.Status;
                order.CreatedBy = user;
                order.UserId = user;
                order.CreatonDate = DateTime.Now;
                order.OrderItems = model.OrderItemModels.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                }).ToList();
                await _orderManager.Add(order);
                await _orderManager.SaveChangesAsync();
                return Ok(new Response<bool>());
            }
            else
            {
                var order = await _orderManager.Get(o => o.Id == model.Id).FirstOrDefaultAsync();
                if (order != null)
                {
                    order.TotalPrice = model.TotalPrice;
                    order.Status = model.Status;
                    order.LastUpdateBy = user;
                    order.LastUpdateDate = DateTime.Now;
                    order.OrderItems.Clear();  // Clear existing items
                    order.OrderItems = model.OrderItemModels.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    }).ToList();
                    _orderManager.Update(order);
                    await _orderManager.SaveChangesAsync();
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
            var order=await _orderManager.Get(o=>o.Id == id).FirstOrDefaultAsync();
            if(order != null)
            {
                await _orderManager.Delete(order);
                await _orderManager.SaveChangesAsync();
                return Ok(new Response<bool>());
            }
            return NotFound(new Response<bool>("Not Found Order"));
        }
    }
}

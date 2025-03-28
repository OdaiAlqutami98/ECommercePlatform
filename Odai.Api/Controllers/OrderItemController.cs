using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odai.Domain.Entities;
using Odai.Logic.Manager;
using Odai.Shared.Auth;
using Odai.Shared.Models;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly OrderItemManager _orderItemManager;
        private readonly OrderManager _orderManager;
        public OrderItemController(OrderItemManager orderItemManager, OrderManager orderManager)
        {
            _orderItemManager = orderItemManager;
            _orderManager = orderManager;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult>GetAll()
        {
            var orderItem = await _orderItemManager.GetAll().Include(o=>o.Product).ToListAsync();
            if (orderItem != null)
            {
                return Ok(orderItem);
            }
            return BadRequest(false);
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult>GetById(int id)
        {
            var orderItem = await _orderItemManager.GetById(id);
            if (orderItem != null)
            {
                return Ok(orderItem);
            }
            return BadRequest(false);
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult>AddEdit(OrderItemModel model)
        {
            var order = await _orderManager.Get(o => o.Id == model.OrderId).Include(o => o.OrderItems).FirstOrDefaultAsync();
            if (model.Id == null)
            {
                OrderItem orderItem=new OrderItem();
                orderItem.Quantity = model.Quantity;
                orderItem.OrderId = model.OrderId;
                orderItem.ProductId = model.ProductId;
                orderItem.UnitPrice=model.UnitPrice;
                orderItem.CreatonDate=DateTime.Now;
                await _orderItemManager.Add(orderItem);
                order.TotalPrice = order.OrderItems.Sum(item => item.Quantity * item.UnitPrice);
                _orderManager.Update(order);
                await _orderItemManager.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Order Item Added successfully.", Data = true });
            }
            else
            {
                var orderItem = await _orderItemManager.Get(oi => oi.Id == model.Id).FirstOrDefaultAsync();
                if (orderItem != null)
                {
                    orderItem.OrderId = model.OrderId;
                    orderItem.ProductId = model.ProductId;
                    orderItem.Quantity = model.Quantity;
                    orderItem.UnitPrice = model.UnitPrice;
                    orderItem.LastUpdateDate = DateTime.Now;
                    _orderItemManager.Update(orderItem);

                    order.TotalPrice = order.OrderItems.Sum(item => item.Quantity * item.UnitPrice);
                    _orderManager.Update(order);
                    await _orderItemManager.SaveChangesAsync();
                    return Ok(new Response<bool> { Succeeded = true, Message = "Order Item updated successfully.", Data = true });
                }
                else
                {
                    return NoContent();
                }
            }
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {

            var orderItem = await _orderItemManager.Get(oi => oi.Id == id).Include(oi=>oi.Order).FirstOrDefaultAsync();
            if (orderItem != null)
            {
                var order = orderItem.Order;
                await _orderItemManager.Delete(orderItem);

                order.TotalPrice -= (orderItem.Quantity * orderItem.UnitPrice);

                _orderManager.Update(order);

                await _orderItemManager.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Order Item Deleted successfully.", Data = true });
            }
            return NotFound(" Not Found OrderItem");
        }
    }
}

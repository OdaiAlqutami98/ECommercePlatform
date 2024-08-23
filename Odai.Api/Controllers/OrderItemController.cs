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
    public class OrderItemController(OrderItemManager _orderitemmanager) : ControllerBase
    {
        //private readonly OrderItemManager _orderitemmanager;
        //public OrderItemController(OrderItemManager orderItemManager)
        //{
        //    _orderitemmanager = orderItemManager;
        //}
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult>GetAll()
        {
            var orderitem=await _orderitemmanager.GetAll().Include(o=>o.Product).OrderBy(o=>o.Order).ToListAsync();
            if (orderitem!=null)
            {
                return Ok(orderitem);
            }
            return BadRequest(false);
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult>GetById(int id)
        {
            var orderitem = await _orderitemmanager.GetById(id);
            if (orderitem!=null)
            {
                return Ok(orderitem);
            }
            return BadRequest(false);
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult>AddEdit(OrderItemModel model)
        {
            if(model.Id==null)
            {
                OrderItem order=new OrderItem();
                order.Quantity = model.Quantity;
                order.OrderId = model.OrderId;
                order.ProductId = model.ProductId;
                order.CreatonDate=DateTime.Now;
                await _orderitemmanager.Add(order);
                await _orderitemmanager.SaveChangesAsync();
                return Ok(new Response<bool>());
            }
            else
            {
                var orderitem=await _orderitemmanager.Get(oi=>oi.Id==model.Id).FirstOrDefaultAsync();
                if (orderitem!=null)
                {
                    OrderItem orderItem=new OrderItem();
                    orderitem.OrderId = model.OrderId;
                    orderitem.ProductId = model.ProductId;
                    orderitem.Quantity = model.Quantity;
                    orderitem.LastUpdateDate = DateTime.Now;
                     _orderitemmanager.Update(orderitem);
                    await _orderitemmanager.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(int id)
        {
            var orderitem= await _orderitemmanager.Get(oi=>oi.Id==id).FirstOrDefaultAsync();
            if (orderitem!=null)
            {
                await _orderitemmanager.Delete(orderitem);
                await _orderitemmanager.SaveChangesAsync();
                return Ok(new Response<bool>());
            }
            return NotFound(" Not Found OrderItem");
        }
    }
}

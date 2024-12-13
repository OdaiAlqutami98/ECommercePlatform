using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odai.Domain;
using Odai.Domain.Enums;
using Odai.Logic.Common.Interface;
using Odai.Logic.Manager;
using Odai.Shared;
using Odai.Shared.Auth;
using Odai.Shared.Table;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController: ControllerBase
    {
        private readonly OrderManager _orderManager;
        public OrderController(OrderManager orderManager)
        {
            _orderManager = orderManager;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<TableResponse<Order>>GetAll(int pageIndex,int pageSize)
        {
            var query =  _orderManager.GetAll().Include(o=>o.OrderItems);
            var length = query.Count();
            var orders = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
           
            return new TableResponse<Order>
            {
                RecordsTotal = length,
                Data = orders
            };

        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult>GetById(int id)
        {
            var order = await _orderManager.GetById(id);
            if (order is not null)
            {
                return Ok(order);
            }
            return BadRequest(false);
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult>AddEdit(OrderModel model)
        {
          
            if (model.Id == null)
            {
                Order order=new Order();
                order.TotalPrice = model.OrderItems?.Sum(item => item.Quantity * item.UnitPrice) ?? 0;
                order.Status = (OrderStatus?)model.Status;
                order.UserId = model.UserId;
                order.OrderItems = model.OrderItems?.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                }).ToList();
                order.CreatedBy = model.UserId;
                order.CreatonDate = DateTime.Now;
                await _orderManager.Add(order);
                await _orderManager.SaveChangesAsync();
                return Ok(new Response<bool>{ Succeeded = true, Message = "Order added successfully.", Data = true });
            }
            else
            {
                var order = await _orderManager.Get(o => o.Id == model.Id).Include(o => o.OrderItems).FirstOrDefaultAsync();

                if (order != null)
                {
                    order.TotalPrice = model.OrderItems?.Sum(item => item.Quantity * item.UnitPrice) ?? 0; 
                    order.Status = (OrderStatus)model.Status;
                    order.LastUpdateBy = model.UserId;
                    order.LastUpdateDate = DateTime.Now;
                    order.OrderItems?.Clear();  // Clear existing items
                    if (model.OrderItems != null)
                    {
                        order.OrderItems = model.OrderItems.Select(item => new OrderItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            CreatonDate = DateTime.Now
                        }).ToList();
                    }
                    order.TotalPrice = order.OrderItems.Sum(item => item.Quantity * item.UnitPrice);

                    _orderManager.Update(order);
                    await _orderManager.SaveChangesAsync();
                    return Ok(new Response<bool> { Succeeded = true, Message = "Order updated successfully.", Data = true });
                }
               
            }
            return BadRequest(false);
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

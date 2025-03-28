using Odai.DataModel;
using Odai.Domain.Entities;
using Odai.Logic.Common;

namespace Odai.Logic.Manager
{
    public class OrderItemManager:BaseManager<OrderItem,OdaiDbContext>
    {
        public OrderItemManager(OdaiDbContext context):base(context)
        {
            
        }
    }
}

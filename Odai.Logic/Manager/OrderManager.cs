using Odai.DataModel;
using Odai.Domain.Entities;
using Odai.Logic.Common;

namespace Odai.Logic.Manager
{
    public class OrderManager:BaseServiceIdentity<Order,OdaiDbContext>
    {
        public OrderManager(OdaiDbContext context):base(context)
        {
          
        }
    }
}

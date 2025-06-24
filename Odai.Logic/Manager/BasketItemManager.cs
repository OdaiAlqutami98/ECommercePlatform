using Odai.DataModel;
using Odai.Domain.Entities;
using Odai.Logic.Common;

namespace Odai.Logic.Manager
{
    public class BasketItemManager: BaseServiceIdentity<BasketItem,OdaiDbContext>
    {
        public BasketItemManager(OdaiDbContext context):base(context)
        {
            
        }
    }
}

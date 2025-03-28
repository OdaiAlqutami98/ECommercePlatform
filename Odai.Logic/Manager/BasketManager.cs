using Odai.DataModel;
using Odai.Domain.Entities;
using Odai.Logic.Common;

namespace Odai.Logic.Manager
{
    public class BasketManager:BaseManager<Basket,OdaiDbContext>
    {
        public BasketManager(OdaiDbContext context):base(context)
        {
            
        }
    }
}

using Odai.DataModel;
using Odai.Domain;
using Odai.Logic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Logic.Manager
{
    public class BasketItemManager:BaseManager<BasketItem,OdaiDbContext>
    {
        public BasketItemManager(OdaiDbContext context):base(context)
        {
            
        }
    }
}

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
    public class BasketManager:BaseManager<Basket,OdaiDbContext>
    {
        public BasketManager(OdaiDbContext context):base(context)
        {
            
        }
    }
}

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
    public class RatingManager:BaseManager<Rating,OdaiDbContext>
    {
        public RatingManager(OdaiDbContext context):base(context) 
        { 
        }
    }
}

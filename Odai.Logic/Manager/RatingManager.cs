using Odai.DataModel;
using Odai.Domain.Entities;
using Odai.Logic.Common;

namespace Odai.Logic.Manager
{
    public class RatingManager:BaseServiceIdentity<Rating,OdaiDbContext>
    {
        public RatingManager(OdaiDbContext context):base(context) 
        { 
        }
    }
}

using ECommercePlatform.Domain.Entities;
using Odai.DataModel;
using Odai.Logic.Common;

namespace ECommercePlatform.Logic.Manager
{
    public  class ClientsManager:BaseServiceIdentity<Clients,OdaiDbContext>
    {
        public ClientsManager(OdaiDbContext context):base(context)
        {
        }
    }
}

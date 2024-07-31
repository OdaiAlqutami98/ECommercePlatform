using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odai.DataModel;

namespace Odai.Logic
{
    public class DependencyInjection
    {
        public DependencyInjection(IServiceCollection services,IConfiguration config)
        {
            services.AddDbContext<OdaiDbContext>(option =>
            {
                option.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
        }
    }
}

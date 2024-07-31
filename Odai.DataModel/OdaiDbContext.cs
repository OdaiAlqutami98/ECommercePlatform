using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Odai.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.DataModel
{
    public class OdaiDbContext:IdentityDbContext<ApplicationUser>
    {
        public OdaiDbContext(DbContextOptions<OdaiDbContext> options):base(options)
        {
            
        }
    }
}

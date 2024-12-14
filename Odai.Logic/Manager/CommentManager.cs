using Odai.DataModel;
using Odai.Domain.Entities;
using Odai.Logic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Logic.Manager
{
    public class CommentManager:BaseManager<Comment,OdaiDbContext>
    {
        public CommentManager(OdaiDbContext dbContext):base(dbContext)
        {
        }
    }
}

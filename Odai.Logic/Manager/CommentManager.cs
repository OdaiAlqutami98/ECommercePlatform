﻿using Odai.DataModel;
using Odai.Domain.Entities;
using Odai.Logic.Common;

namespace Odai.Logic.Manager
{
    public class CommentManager:BaseManager<Comment,OdaiDbContext>
    {
        public CommentManager(OdaiDbContext dbContext):base(dbContext)
        {
        }
    }
}

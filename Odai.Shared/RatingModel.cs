﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Shared
{
    public class RatingModel
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public int Value { get; set; }
    }
}

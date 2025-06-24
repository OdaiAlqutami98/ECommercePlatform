using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommercePlatform.Domain.Identity;
using Odai.Domain.Common;
using Odai.Domain.Entities;

namespace ECommercePlatform.Domain.Entities
{
    public class UserType:BaseEntity
    {
        public string Name { get; set; }
    }
}

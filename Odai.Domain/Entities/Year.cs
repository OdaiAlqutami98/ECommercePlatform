using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odai.Domain.Common;
using Odai.Domain.Entities;

namespace ECommercePlatform.Domain.Entities
{
    public class Year : BaseEntity
    {
        [Column("Name")]
        [Range(2000,int.MaxValue,ErrorMessage ="the year must be greater than or equal to 2000.")]
        public int Name { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}

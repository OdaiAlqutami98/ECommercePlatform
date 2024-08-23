using Odai.Domain.Common;
using Odai.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Domain
{
    public class Order:BaseEntity
    {
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}

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
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public OrderStatus? Status { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}

using Odai.Domain.Common;
using Odai.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odai.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public OrderStatus? Status { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}

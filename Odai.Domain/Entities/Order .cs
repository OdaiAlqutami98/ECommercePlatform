using ECommercePlatform.Domain.Entities;
using Odai.Domain.Common;
using Odai.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odai.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Clients? Clients { get; set; }
        public OrderStatus? Status { get; set; }
        public decimal TotalPrice { get; set; }
        public int? PromoCode { get; set; }
        public int YearId { get; set; }
        [ForeignKey("YearId")]
        public virtual Year? Year  { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}

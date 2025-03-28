using Odai.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odai.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }



    }
}

using Odai.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odai.Domain.Entities
{
    public class BasketItem : BaseEntity
    {
        public int BasketId { get; set; }
        [ForeignKey("BasketId")]
        public Basket? Basket { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

    }
}

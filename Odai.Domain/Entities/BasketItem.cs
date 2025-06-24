using ECommercePlatform.Domain.Entities;
using ECommercePlatform.Domain.Identity;
using Odai.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odai.Domain.Entities
{
    public class BasketItem : BaseEntity
    {
        public int BasketId { get; set; }
        [ForeignKey("BasketId")]
        public virtual Basket? Basket { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Clients? Clients { get; set; }

    }
}

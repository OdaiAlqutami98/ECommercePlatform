using ECommercePlatform.Domain.Entities;
using ECommercePlatform.Domain.Identity;
using Odai.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odai.Domain.Entities
{
    public class Basket : BaseEntity
    {
        
        public int? ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Clients? Clients { get; set; }
        public virtual ICollection<BasketItem>? BasketItems { get; set; }
    }
}

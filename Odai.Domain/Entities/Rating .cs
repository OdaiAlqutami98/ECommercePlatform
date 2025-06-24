using ECommercePlatform.Domain.Entities;
using ECommercePlatform.Domain.Identity;
using Odai.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odai.Domain.Entities
{
    public class Rating : BaseEntity
    {
        public int Value { get; set; }  // Rating value (e.g., 1-5)
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Clients? Clients { get; set; }
    }
}

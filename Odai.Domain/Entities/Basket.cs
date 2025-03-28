using Odai.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odai.Domain.Entities
{
    public class Basket : BaseEntity
    {
        [ForeignKey("UserId")]
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public ICollection<BasketItem>? BasketItems { get; set; }
    }
}

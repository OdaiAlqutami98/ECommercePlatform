using Odai.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Domain
{
    public class Basket :BaseEntity
    {
        [ForeignKey("UserId")]
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public ICollection<BasketItem>? BasketItems { get; set; }
    }
}

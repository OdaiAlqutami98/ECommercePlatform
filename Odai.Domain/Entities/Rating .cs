using Odai.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Domain.Entities
{
    public class Rating : BaseEntity
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]

        public Product? Product { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]

        public ApplicationUser? User { get; set; }
        public int Value { get; set; }  // Rating value (e.g., 1-5)
    }
}

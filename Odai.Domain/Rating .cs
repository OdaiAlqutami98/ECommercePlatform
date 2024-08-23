using Odai.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Domain
{
    public class Rating:BaseEntity
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int Value { get; set; }  // Rating value (e.g., 1-5)
        public string? Comment { get; set; }
    }
}

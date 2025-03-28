using Odai.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odai.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        public string? Content { get; set; }
    }
}

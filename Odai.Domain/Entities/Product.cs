using Odai.Domain.Common;
using Odai.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odai.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        public bool Favorite { get; set; }
        public int? TotalSold { get; set; }
        public ProductStatus Status { get; set; }
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }
        public int Stock { get; set; }
        public decimal? Discount { get; set; }
       
    }
}

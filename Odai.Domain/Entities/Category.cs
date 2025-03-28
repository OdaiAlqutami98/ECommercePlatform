using Odai.Domain.Common;

namespace Odai.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}

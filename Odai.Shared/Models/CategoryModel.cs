using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Odai.Shared.Models
{
    public class CategoryModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public IFormFile? ImagePath { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<ProductModel>? Products { get; set; }
    }


}

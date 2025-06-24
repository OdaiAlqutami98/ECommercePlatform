using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Odai.Shared.Models
{
    public class ProductModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        [JsonIgnore] // So that it doesn't return null in GET
        public IFormFile? ImagePath { get; set; }
        public string? ImageUrl { get; set; }
        public bool Favorite { get; set; }
        public int Status { get; set; }
        public int Stock { get; set; }
        public decimal? Discount { get; set; }
        public ICollection<CommentModel>? Comments { get; set; }
        public ICollection<RatingModel>? Ratings { get; set; }
    }

}

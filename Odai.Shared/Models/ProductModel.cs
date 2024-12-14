using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Shared.Models
{
    public class ProductModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? ImagePath { get; set; }
        public bool Favorite { get; set; }
        public int Status { get; set; }
        public int Stock { get; set; }
        public ICollection<CommentModel>? Comments { get; set; }
        public ICollection<RatingModel>? Ratings { get; set; }
    }

}

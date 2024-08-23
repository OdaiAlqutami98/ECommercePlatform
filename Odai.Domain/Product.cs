using Microsoft.AspNetCore.Http;
using Odai.Domain.Common;
using Odai.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Odai.Domain
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }
        [DisplayName("Category")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public bool Favorite { get; set; }
        public Status Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Odai.Domain.Common;

namespace Odai.Domain
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }
        public List<Product> Products { get; set; }
    }
}

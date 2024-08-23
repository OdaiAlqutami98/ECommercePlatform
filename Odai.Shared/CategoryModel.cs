using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Shared
{
    public record CategoryModel
        (
        int? Id,
        string Name,
        List<ProductModel> Products,
        IFormFile? ImagePath
        );
    //{
    //    public int? Id { get; set; }
    //    public string Name { get; set; }
    //    public List<ProductModel> Products { get; set; }
    //    public IFormFile? ImagePath { get; set; }
    //}
}

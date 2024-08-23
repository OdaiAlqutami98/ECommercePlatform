using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Shared
{
    public record ProductModel
   (
         int? Id ,
         string Name, 
         string Description, 
         decimal Price ,
         int CategoryId ,
         IFormFile? ImagePath, 
         bool Favorite ,
         string Status

    );
}

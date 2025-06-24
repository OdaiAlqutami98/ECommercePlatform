using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odai.Domain.Common;

namespace ECommercePlatform.Domain.Entities
{
   public class Settings:BaseEntity
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }
    }
}

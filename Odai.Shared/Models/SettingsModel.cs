using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ECommercePlatform.Shared.Models
{
    public class SettingsModel
    {
        public int? Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        [JsonIgnore]
        public IFormFile? ImagePath { get; set; }
        public string? ImageUrl { get; set; }
    }
}

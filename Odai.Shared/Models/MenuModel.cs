using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommercePlatform.Shared.Models
{
   public class MenuModel
    {
        public int Id { get; set; }
        public string? NameKey { get; set; }
        public int Level { get; set; }
        public int Order { get; set; }
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int? ParentId { get; set; }
        public List<MenuModel>? SubItems { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Shared
{
    public class BasketModel
    {
        public int? Id { get; set; }
        public Guid? UserId { get; set; }
        public ICollection<BasketItemModel>? BasketItems { get; set; }
    }
       
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Shared
{
    public class BasketItemModel
    {
        public int? Id { get; set; }
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid UserId { get; set; }
        public decimal UnitPrice { get; set; }
    }
      
}

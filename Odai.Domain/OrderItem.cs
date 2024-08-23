using Odai.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Domain
{
    public class OrderItem:BaseEntity
    {
        [ForeignKey("Product")]
        [DisplayName("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("Order")]
        [DisplayName("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        
    }
}

using Odai.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Domain
{
    public class BasketItem:BaseEntity
    {
        [ForeignKey("Basket")]
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }// معرف المستخدم الذي أضاف العنصر للسلة

    }
}

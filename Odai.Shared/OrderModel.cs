using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Shared
{
    public record OrderModel
        (
         int? Id,
         Guid UserId,
        // [DeniedValues("A")]
         string Status,
         decimal TotalPrice,
          List<OrderItemModel> OrderItemModels
        );
    //{
    //    public int? Id { get; set; }
    //    public Guid? UserId { get; set; }
    //    public string Status { get; set; }
    //    public decimal TotalPrice { get; set; }
    //    public List<OrderItemModel> OrderItemModels { get; set; }
    //}
}

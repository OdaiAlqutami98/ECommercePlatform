using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Shared
{
    public record OrderItemModel
        (
        int? Id,
        int ProductId,
        int Quantity,
        int OrderId
        );
    //{
    //    public int? Id { get; set; }
    //    public int ProductId { get; set; }
    //    public int Quantity { get; set; }
    //    public int OrderId { get; set; }
    //}
}

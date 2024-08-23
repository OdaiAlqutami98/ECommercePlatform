using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Shared
{
    public record BasketItemModel
        (
        int? Id,
        int BasketId,
        int ProductId,
        int Quantity,
         Guid UserId
        );
    //{
    //    public int? Id { get; set; }
    //    public int BasketId { get; set; }
    //    public int ProductId { get; set; }
    //    public int Quantity { get; set; }
    //}
}

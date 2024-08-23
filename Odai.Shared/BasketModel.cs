using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Shared
{
    public record BasketModel
        (
        int? Id,
        Guid UserId, 
        List<BasketItemModel> BasketItems
        );
    //{
    //    public int? Id { get; set; }
    //    public Guid UserId { get; set; }
    //    public List<BasketItemModel> BasketItems { get; set; }
    //}
}

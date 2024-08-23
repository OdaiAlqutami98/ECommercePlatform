using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Shared
{
    public record RatingModel
        (
        int? Id,
        int ProductId,
        string? Comment,
        Guid UserId,
        int Value
        );
    //{
    //    public int? Id { get; set; }
    //    public int ProductId { get; set; }
    //    public string? Comment { get; set; }
    //    public Guid UserId { get; set; }
    //    public int Value { get; set; }  // Rating value (e.g., 1-5)
    //}
}

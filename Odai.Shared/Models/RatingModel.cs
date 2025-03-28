
namespace Odai.Shared.Models
{
    public class RatingModel
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public int Value { get; set; }
    }
}

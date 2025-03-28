
namespace Odai.Shared.Models
{
    public class OrderModel
    {
        public int? Id { get; set; }
        public Guid UserId { get; set; }
        public int Status { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderItemModel>? OrderItems { get; set; }
    }
}

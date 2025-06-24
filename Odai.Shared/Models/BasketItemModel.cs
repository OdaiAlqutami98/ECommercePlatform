
namespace Odai.Shared.Models
{
    public class BasketItemModel
    {
        public int? Id { get; set; }
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int ClientId { get; set; }
        public decimal UnitPrice { get; set; }
    }

}

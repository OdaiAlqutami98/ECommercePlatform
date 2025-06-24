
namespace Odai.Shared.Models
{
    public class BasketModel
    {
        public int? Id { get; set; }
        public int? ClientId { get; set; }
        public ICollection<BasketItemModel>? BasketItems { get; set; }
    }

}

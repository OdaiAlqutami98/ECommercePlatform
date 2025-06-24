using Odai.Shared.Models;

namespace ECommercePlatform.Shared.Models
{
   public class ClientsModel
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string? BuildNumber { get; set; }
        public string Address { get; set; }
        public string? StreetName { get; set; }
        public string? Comment { get; set; }
        public int Payment { get; set; }
        public int City { get; set; }
        public ICollection<OrderModel>? Orders { get; set; }
    }
}

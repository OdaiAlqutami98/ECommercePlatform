using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odai.Domain.Common;
using Odai.Domain.Entities;
using Odai.Domain.Enums;

namespace ECommercePlatform.Domain.Entities
{
    public class Clients:BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string? BuildNumber { get; set; }
        public string Address { get; set; }
        public string? StreetName { get; set; }
        public string? Comment { get; set; }
        public City City { get; set; }
        public Payment Payment { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Basket>? Baskets { get; set; }
        public virtual ICollection<BasketItem>? BasketItems { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Rating>? Ratings { get; set; }


    }
}

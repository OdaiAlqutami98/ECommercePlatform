using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ECommercePlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommercePlatform.Domain.Identity
{
    [Table("AspNetUsers")]
    public partial class User:IdentityUser<Guid>
    {
        public override Guid Id {  get; set; }
        public int? UserTypeId { get; set; }
        [NotMapped]
        public virtual UserType? UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        [SwaggerSchema("IsDeleted",ReadOnly=true)]
        public virtual int IsDeleted { get; set; }
    }
}

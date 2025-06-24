using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommercePlatform.Domain.AccessControl;
using Microsoft.AspNetCore.Identity;

namespace ECommercePlatform.Domain.Identity
{
    [Table("AspNetRoles")]
    public class Role : IdentityRole<Guid>
    {
        [Key]
        [Column("Id")]
        public override Guid Id { get; set; }
        public Role() { }
        public Role(string roleName) : base(roleName) { }
        public virtual ICollection<RoleMenuItems>? RolePages { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using ECommercePlatform.Domain.Identity;

namespace ECommercePlatform.Domain.AccessControl
{
    public class RoleMenuItems
    {
        [Key]
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public virtual Menu MenuItem { get; set; }
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}

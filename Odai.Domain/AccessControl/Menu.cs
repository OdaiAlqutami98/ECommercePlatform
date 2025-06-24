using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommercePlatform.Domain.AccessControl
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        public string? NameKey { get; set; }
        public int Level { get; set; }
        public int Order { get; set; }
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Menu? Parent { get; set; }
        public virtual ICollection<RoleMenuItems>? RolePages { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ECommercePlatform.Shared.Auth
{
    public class RegisterModel
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string  LastName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? UserTypeId { get; set; }
        public List<string>? UserRoles { get; set; }
        [NotMapped]
        public string? RoleName { get; set; }
    }
}

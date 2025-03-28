
namespace Odai.Shared.Auth
{
    public class RegisterResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? role { get; set; }
        public string Name { get; set; }
        public bool IsVerified { get; set; }
        public string Token { get; set; }
    }
}

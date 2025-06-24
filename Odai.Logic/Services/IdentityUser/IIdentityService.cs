using ECommercePlatform.Domain.Identity;
using ECommercePlatform.Shared.Auth;
using Odai.Shared.Auth;

namespace ECommercePlatform.Logic.Services.IdentityUser
{
    public interface IIdentityService
    {
        Task<User> GetUserAsync(Guid userId);
        Task<Response<string>> DeleteUser(Guid userId);
        Task<bool> IsInRoleAsync(Guid userId, string role);
        Task<Response<RegisterResponse>> AuthenticateAsync(LoginRequstt request);
        Task<Response<User>> CreateUserAsync(string userName, string password);
        Task<Response<User>> Register(RegisterModel model);
        Task<Response<User>> RegisterAdmin(RegisterModel model);
        Task<Response<User>> UpdateUser(RegisterModel model);
        Task<List<string>> FindUserRolesAsync(string userName);
    }
}

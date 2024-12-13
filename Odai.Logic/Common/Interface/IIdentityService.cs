using Odai.Domain;
using Odai.Domain.Enums;
using Odai.Shared;
using Odai.Shared.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Odai.Logic.Common.Service.IdentityService;

namespace Odai.Logic.Common.Interface
{
    public interface IIdentityService
    {
        Task<ApplicationUser> GetUserAsync(Guid userId);
        Task<bool> IsInRoleAsync(Guid userId, string role);
        Task<Shared.Auth.Response<RegisterResponse>> AuthenticateAsync(LoginRequstt request);
        Task<Shared.Auth.Response<ApplicationUser>> CreateUserAsync(string userName, string password);
        Task<Shared.Auth.Response<ApplicationUser>> RegisterUserAsync(ApplicationUserModel model);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<List<UserWithRoles>> GetUserRolesAsync();
        Task<Shared.Auth.Response<string>> UpdateUserRolesAsync(Guid userId, List<string> roles);

    }
}

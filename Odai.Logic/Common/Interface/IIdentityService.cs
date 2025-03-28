using Odai.Domain.Entities;
using Odai.Domain.Enums;
using Odai.Shared.Auth;
using Odai.Shared.Models;
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
        Task<Response<string>> DeleteUser(Guid userId);
        Task<bool> IsInRoleAsync(Guid userId, string role);
        Task<Response<RegisterResponse>> AuthenticateAsync(LoginRequstt request);
        Task<Response<ApplicationUser>> CreateUserAsync(string userName, string password);
        Task<Response<ApplicationUser>> RegisterUserAsync(ApplicationUserModel model);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<Response<string>> UpdateUserRolesAsync(Guid userId, List<string> roles);

    }
}

using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Odai.DataModel;
using Odai.Domain.Entities;
using Odai.Domain.Enums;
using Odai.Logic.Common.Interface;
using Odai.Shared.Auth;
using Odai.Shared.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Logic.Common.Service
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly OdaiDbContext _context;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IConfiguration _config;
        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            OdaiDbContext context,
            RoleManager<IdentityRole<Guid>> roleManager,
            IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }
        public async Task<Shared.Auth.Response<ApplicationUser>> RegisterUserAsync(ApplicationUserModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Name,
                Email = model.Email,
                EmailConfirmed = true,
                Role = Role.User
            };
            // Ensure roles exist in the system
            string[] roleNames = { "Owner", "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles and seed them to the database
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, Role.User.ToString());
            if (result.Succeeded)
            {
                return new Shared.Auth.Response<ApplicationUser>(user);
            }
            return new Shared.Auth.Response<ApplicationUser>($"Accounts Registered Before {model.Name}");
        }
        public async Task<Shared.Auth.Response<RegisterResponse>> AuthenticateAsync(LoginRequstt request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return new Shared.Auth.Response<RegisterResponse>($"No Accounts Registered with '{request.Username}'");
            }
            if (!user.EmailConfirmed)
            {
                return new Shared.Auth.Response<RegisterResponse>($"Email Not Confirmed '{request.Username}'");
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return new Shared.Auth.Response<RegisterResponse>($"Invalid Credentials for '{request.Username}'.");
            }
            var response = await GetAuthenticationResponseAsync(user);
            return new Shared.Auth.Response<RegisterResponse>(response, $"Authentacated {user.UserName}");


        }
        public async Task<RegisterResponse> GetAuthenticationResponseAsync(ApplicationUser user)
        {
            // generate new jwt
            JwtSecurityToken jwtSecurityToken = await GenerateJwToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);
            var role = rolesList.FirstOrDefault();


            var response = new RegisterResponse
            {
                Name = user.UserName,
                UserId = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                role = role,
                IsVerified = user.EmailConfirmed,
            };
            return response;
        }
        private async Task<JwtSecurityToken> GenerateJwToken(ApplicationUser user)
        {


            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                  new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.Email, user.Email),
                  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //signingCredentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
               expires: DateTime.UtcNow.AddDays(30),
               signingCredentials: signingCredentials
               );
            return token;
        }
        public async Task<Shared.Auth.Response<ApplicationUser>> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true,
                Role = Role.Admin,
            };
            // Ensure roles exist in the system
            string[] roleNames = { "Owner", "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles and seed them to the database
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
            var result = await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, "Admin");
            return new Shared.Auth.Response<ApplicationUser>(user);
        }

        public async Task<ApplicationUser> GetUserAsync(Guid userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }
        public async Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return await _userManager.IsInRoleAsync(user, role);
        }
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<Shared.Auth.Response<string>> UpdateUserRolesAsync(Guid userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Shared.Auth.Response<string>("User Not Found");
            }
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return new Shared.Auth.Response<string>("Failed to remove current roles");
            }
            var addResult = await _userManager.AddToRolesAsync(user, roles);
            if (!addResult.Succeeded)
            {
                return new Shared.Auth.Response<string>("Failed to add new roles");
            }
            return new Shared.Auth.Response<string>("User roles update successfully");
        }

        public async Task <Shared.Auth.Response<string>> DeleteUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null) 
            {
               await _userManager.DeleteAsync(user);
               return new Shared.Auth.Response<string>("User deleted successfully");
            }
            return new Shared.Auth.Response<string>("User Not Found");
        }
    }
}

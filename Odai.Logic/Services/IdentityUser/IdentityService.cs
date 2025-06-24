
using ECommercePlatform.Domain.Identity;
using ECommercePlatform.Shared.Auth;
using ECommercePlatform.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Odai.DataModel;
using Odai.Shared.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommercePlatform.Logic.Services.IdentityUser
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly OdaiDbContext _context;
        private readonly RoleManager<Domain.Identity.Role> _roleManager;
        private readonly IConfiguration _config;
        public IdentityService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            OdaiDbContext context,
            RoleManager<Domain.Identity.Role> roleManager,
            IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }
        public async Task<Response<User>> RegisterAdmin(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                return new Response<User>("User already exists!", HttpStatusCodes.InternalServerError);
            }

            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
            {
                return new Response<User>("Email already exists!", HttpStatusCodes.InternalServerError);
            }

            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                UserTypeId = model.UserTypeId,
                //RoleName = model.RoleName

            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + "\n";
                }
                return new Response<User>("Admin creation failed! Please check user details and try again.\n" + errors, HttpStatusCodes.InternalServerError);
            }

            //if (!await _roleManager.RoleExistsAsync(UserRole.Admin))
            //    await _roleManager.CreateAsync(new Domain.Identity.Role(UserRole.Admin));

            //if (await _roleManager.RoleExistsAsync(model.RoleName))
            //{
            //    await _userManager.AddToRoleAsync(user, model.RoleName);
            //}

            await _userManager.AddToRolesAsync(user, model.UserRoles);

            return new Response<User>(user, HttpStatusCodes.OK,"Admin created successfully!" );
        }
        public async Task<Response<User>> Register([FromBody] RegisterModel model)
        {
            var userExist = await _userManager.FindByNameAsync(model.UserName);
            if (userExist != null)
            {
                return new Response<User>("User already exists!", HttpStatusCodes.InternalServerError);
            }
            var emailExist = await _userManager.FindByEmailAsync(model.Email);
            if (emailExist != null)
            {
                return new Response<User>("Email already exists!", HttpStatusCodes.InternalServerError);
            }
            User user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                //Role = Role.User
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return new Odai.Shared.Auth.Response<User>("User creation failed! Please check user details and try again.", HttpStatusCodes.InternalServerError);
            }

            if (!await _roleManager.RoleExistsAsync(UserRole.User))
                await _roleManager.CreateAsync(new Domain.Identity.Role(UserRole.User));

            if (await _roleManager.RoleExistsAsync(UserRole.User))
            {
                await _userManager.AddToRoleAsync(user, UserRole.User);
            }

            return new Odai.Shared.Auth.Response<User>($"User created successfully! {model.UserName}", HttpStatusCodes.OK);
        }
        public async Task<Odai.Shared.Auth.Response<RegisterResponse>> AuthenticateAsync(LoginRequstt request)
        {
            var user = await _userManager.FindByEmailAsync(request.Username);
            if (user == null)
            {
                return new Odai.Shared.Auth.Response<RegisterResponse>($"No Accounts Registered with '{request.Username}'", HttpStatusCodes.BadRequest);
            }
            if (!user.EmailConfirmed)
            {
                return new Odai.Shared.Auth.Response<RegisterResponse>($"Email Not Confirmed '{request.Username}'", HttpStatusCodes.BadRequest);
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return new Odai.Shared.Auth.Response<RegisterResponse>($"Invalid Credentials for '{request.Username}'.", HttpStatusCodes.BadRequest);
            }
            var response = await GetAuthenticationResponseAsync(user);
            return new Odai.Shared.Auth.Response<RegisterResponse>(response, HttpStatusCodes.OK, $"Authentacated {user.UserName}");


        }
        public async Task<RegisterResponse> GetAuthenticationResponseAsync(User user)
        {
            // generate new jwt
            JwtSecurityToken jwtSecurityToken = await GenerateJwToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);
            var role = rolesList.FirstOrDefault();


            var response = new RegisterResponse
            {
                UserId = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                IsVerified = user.EmailConfirmed,
            };
            return response;
        }
        private async Task<JwtSecurityToken> GenerateJwToken(User user)
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
                claims.Add(new Claim("role", role));

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
        public async Task<Odai.Shared.Auth.Response<User>> CreateUserAsync(string userName, string password)
        {
            var user = new User
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true,
                //Role = Role.Admin,
            };
            // Ensure roles exist in the system
            string[] roleNames = { "Owner", "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles and seed them to the database
                    var roleResult = await _roleManager.CreateAsync(new Domain.Identity.Role(roleName));
                }
            }
            var result = await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, "Admin");
            return new Odai.Shared.Auth.Response<User>(user, HttpStatusCodes.Created);
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }
        public async Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return await _userManager.IsInRoleAsync(user, role);
        }


        public async Task<Odai.Shared.Auth.Response<string>> DeleteUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return new Odai.Shared.Auth.Response<string>("User deleted successfully", HttpStatusCodes.OK);
            }
            return new Odai.Shared.Auth.Response<string>("User Not Found", HttpStatusCodes.NotFound);
        }

        public async Task<Odai.Shared.Auth.Response<User>> UpdateUser(RegisterModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                return new Odai.Shared.Auth.Response<User>("User not found!", HttpStatusCodes.NotFound);
            }
            var emailExist = await _userManager.FindByEmailAsync(model.Email);
            if (emailExist != null && emailExist.Id != model.Id)
            {
                return new Odai.Shared.Auth.Response<User>("Email already exists!", HttpStatusCodes.BadRequest);
            }
            var userExist = await _userManager.FindByNameAsync(model.UserName);
            if (userExist != null && userExist.Id != model.Id)
            {
                return new Odai.Shared.Auth.Response<User>("User already exists!", HttpStatusCodes.BadRequest);
            }
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.LastName = model.LastName;
            user.FirstName = model.FirstName;
            user.UserTypeId = model.UserTypeId;

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (hasPassword)
            {
                var remove = await _userManager.RemovePasswordAsync(user);
                if (!remove.Succeeded)
                {
                    return new Odai.Shared.Auth.Response<User>(remove.Errors.Select(r => r.Description).ToList(), HttpStatusCodes.BadRequest);

                }
            }
            var addPassword = await _userManager.AddPasswordAsync(user, model.Password);
            if (!addPassword.Succeeded)
            {
                return new Odai.Shared.Auth.Response<User>(addPassword.Errors.Select(a => a.Description).ToList(), HttpStatusCodes.BadRequest);
            }
            //await _userManager.AddToRolesAsync(user, model.UserRoles);



            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new Odai.Shared.Auth.Response<User>(result.Errors.Select(e => e.Description).ToList(), HttpStatusCodes.BadRequest);
            }
            return new Odai.Shared.Auth.Response<User>(user, HttpStatusCodes.OK);

        }

        public async Task<List<string>> FindUserRolesAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return [];
            }

            return [.. (await _userManager.GetRolesAsync(user))];
        }
    }
}


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odai.DataModel;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ECommercePlatform.Domain.Identity;
using ECommercePlatform.Logic.Services.IdentityUser;
using ECommercePlatform.Logic.Services.Menu;
using ECommercePlatform.Logic.Services.RoleMenuItems;
using ECommercePlatform.Logic.Services.Category;
using ECommercePlatform.Logic.Services.Product;
using ECommercePlatform.Logic.Services.UserType;
using ECommercePlatform.Logic.Services.Role;
using ECommercePlatform.Logic.Services.Setting;
using ECommercePlatform.Logic.Services.Report.ExportFactory;
using ECommercePlatform.Logic.Services.Report.ExportService;
using ECommercePlatform.Logic.Services.Report.ExportStrategy;

namespace Odai.Logic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure( this IServiceCollection services,IConfiguration config)
        {
            string IdentityConnString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<OdaiDbContext>(option =>
            {
                option.UseSqlServer(IdentityConnString, builder => builder.MigrationsAssembly(typeof(OdaiDbContext).Assembly.FullName));
            });
            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<OdaiDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme= JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["JwtSettings:Issuer"],
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]))
                };
            });
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddScoped<UserManager<User>>();
            services.AddScoped<SignInManager<User>>();
            services.AddScoped<RoleService>();
            services.AddTransient<IMenuService, MenuService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IRoleMenuItemsService, RoleMenuItemsService>();
            services.AddTransient<IUserTypeService, UserTypeService>();
            services.AddTransient<ISettingService, SettingService>();
            services.AddTransient<ReportExportStrategy>();
            services.AddTransient<PdfExportService>();
            services.AddTransient<ExcelExportService>();
            services.AddTransient<IReportExportStrategy, PdfExportStrategy>();
            services.AddTransient<IReportExportStrategy, ExcelExportStrategy>();

            //services.AddScoped<ProductManager>();
            //services.AddScoped<OrderItemManager>();
            //services.AddScoped<OrderManager>();
            //services.AddScoped<RatingManager>();
            //services.AddScoped<BasketManager>();
            //services.AddScoped<BasketItemManager>();
            //services.AddScoped<CommentManager>();
            //services.AddScoped<UserTypeManager>();



            return services;
        }
    }
}

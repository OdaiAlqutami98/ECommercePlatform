﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odai.DataModel;
using Odai.Logic.Common.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Odai.Logic.Manager;
using Odai.Logic.Common.Service;
using Odai.Domain.Entities;

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
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
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
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>>();
            services.AddScoped<CategoryManager>();
            services.AddScoped<ProductManager>();
            services.AddScoped<OrderItemManager>();
            services.AddScoped<OrderManager>();
            services.AddScoped<RatingManager>();
            services.AddScoped<BasketManager>();
            services.AddScoped<BasketItemManager>();
            services.AddScoped<CommentManager>();


            return services;
        }
    }
}

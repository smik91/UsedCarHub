using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UsedCarHub.BusinessLogic.Interfaces;
using UsedCarHub.BusinessLogic.Services;
using UsedCarHub.Common.Auth;
using UsedCarHub.Common.Interfaces;
using UsedCarHub.Repository.Interfaces;
using UsedCarHub.Repository.Repositories;

namespace UsedCarHub.API.Extensions
{
    public static class ServicesExtension
    {
        private static readonly IConfiguration _configuration;
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void AddCustomAuthentication(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
        }
    }
}